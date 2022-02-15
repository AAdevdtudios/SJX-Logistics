﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using SjxLogistics.Models.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataBaseContext _context;
        public UserController(DataBaseContext context)
        {
            _context = context;
        }

        // Get user profile
        #region Get Users Profile
        [HttpGet]
        public async Task<ActionResult> GetUserProfile()
        {
            var response = new ServiceResponses<Users>();
            try
            {

                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Data = user;
                response.Success = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.StatusCode = 400;
                response.Data = null;
                response.Success = false;
                return BadRequest(response);
            }
        }
        #endregion

        //Get Order Of user
        #region Order List For A User
        [HttpGet("order")]
        public async Task<ActionResult> GetUserOrder()
        {
            var response = new ServiceResponses<ICollection<Order>>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Data = user.Orders;
                response.Success = true;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.StatusCode = 400;
                response.Data = null;
                response.Success = false;
                return BadRequest(response);
            }

        }
        #endregion

        //Create Order
        #region Create Order 
        //Create an Order
        [HttpPost("placeOrder")]
        public async Task<ActionResult> PlaceOrder([FromBody] OrderRequest order)
        {
            //
            var response = new ServiceResponses<Order>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();

                var user = await _context.Users.FindAsync(userId);

                var rand = new Random();
                var uid = rand.Next(1000, 100000);
                string deliveryCode = GenerateDeliveryCode(rand.Next(1000, 10000));
                string orderCode = GetnewId(uid);
                int price = CalculateCharges(order.Distance);
                var responce = PaymentsMethod(order.PaymentType);

                if (price != 0)
                {
                    Order orders = new ()
                    {
                        PickUp = order.PickUp,
                        Delivery = order.Delivery,
                        ReceiversName = order.ReceiversName,
                        Categories = order.Categories,
                        Weight = order.Weight,
                        CustomersEmail = user.Email,
                        OrderCode = orderCode,
                        Status = OrderStatus.Pending,
                        IsExpressDelivery = order.IsExpressDelivery,
                        ReceiversPhone = order.ReceiversPhone,
                        Charges = price,
                        DeliveryCode = deliveryCode,
                        PaymentType = responce,
                        CreatedAt = DateTime.Now.Date,
                    };
                    Notifications notifications = new ()
                    {
                        From = "SJX Logistics",
                        Message = "Your order code is "+orderCode + " please provide this to the rider",
                        MessageType = "Order Placed",
                        CreatedAt = DateTime.Now,
                        Status = NotificationStatus.UnRead
                    };
                    user.Notifications.Add(notifications);
                    _context.Order.Add(orders);
                    orders.Users = user;
                    await _context.SaveChangesAsync();
                    response.Messages = "Successful";
                    response.StatusCode = 200;
                    response.Data = orders;
                    response.Success = true;

                    return Ok(response);
                }
                else
                {
                    response.Messages = "Your distance couldn't be calculated";
                    response.StatusCode = 400;
                    response.Success = false;
                    response.Data = null;
                    return BadRequest(response);
                }
            }
            catch (Exception)
            {
                response.Messages = "Un-able to take your order";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }
        }
        #endregion

        //Cancel Order
        #region Cancel an Order
        [HttpPost("cancelOrder")]
        public async Task<IActionResult> CancelOrder([FromBody]CancelOrderRequest request)
        {
            var response = new ServiceResponses<Order>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();

                var user = await _context.Users.FindAsync(userId);

                var order = user.Orders.FirstOrDefault(i => i.Id == request.Id);
                if(order.Status != OrderStatus.Pending)
                {
                    response.Messages = "Order has already been assigned to a rider";
                    response.Success = false;
                    response.StatusCode = 400;
                    return BadRequest(response);
                }
                order.Status = OrderStatus.Canceled;
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = order;

                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.StatusCode = 400;
                response.Success = false;

                return BadRequest(response);
            }
        }
        #endregion

        //Code 
        #region Code for generating values
        string GetnewId(int ids)
        {
            var value = _context.Order.FirstOrDefault(i => i.OrderCode == "#" + ids);
            if (value != null)
            {
                ids = +1;
            }
            return "#" + ids;
        }
        string GenerateDeliveryCode(int ids)
        {
            var values = _context.Order.FirstOrDefault(i => i.DeliveryCode == "UI" + ids);
            if (values == null)
                ids += 1;
            return "UI" + ids;
        }

        static int CalculateCharges(double distance)
        {
            int basePrice = 500;//1000
            if (distance == 0)
            {
                return 0;
            }
            int value = 0;

            if (distance > 50)
                value = basePrice + 4 * basePrice;
            else if (distance > 40)
                value = basePrice + 2 * basePrice;
            else if (distance > 30)
                value = (int)(basePrice + 1.5 * basePrice);
            else if (distance > 20)
                value = (int)(basePrice + 0.75 * basePrice);
            else if (distance > 10)
                value = (int)(basePrice + 0.5 * basePrice);

            return value;
        }

        static string PaymentsMethod(string orderRequest)
        {
            orderRequest = orderRequest.ToLower() switch
            {
                "transfer" => "Transfer",
                "card" => "Card",
                _ => "Wallet",
            };
            return orderRequest;
        }
        #endregion

        #region Get Status Of Order or all
        //Search for status of orders
        [HttpGet("orders/[action]")]
        public async Task<ActionResult<Order>> Search(string status)
        {
            var response = new ServiceResponses<IQueryable<Order>>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();

                var users = await _context.Users.FindAsync(userId);

                var user = _context.Order.Where(i => i.Users.Id == users.Id);
                if (user == null)
                {
                    return BadRequest();
                }

                IQueryable<Order> order = status.ToLower() switch
                {
                    "pending" => user.Where(i => i.Status == OrderStatus.Pending),
                    "assigned" => user.Where(i => i.Status == OrderStatus.Assigned),
                    "transit" => user.Where(i => i.Status == OrderStatus.Transit),
                    "delivered" => user.Where(i => i.Status == OrderStatus.Delivered),
                    "canceled" => user.Where(i => i.Status == OrderStatus.Canceled),
                    _ => user,
                };

                //
                response.Messages = "Successful";
                response.Success = true;
                response.Data = order;
                response.StatusCode = 200;


                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.Success = false;
                response.Data = null;
                response.StatusCode = 400;
                return BadRequest(response);
            }
        }
        #endregion
        //User Notification
        #region Read All Notifications
        [HttpGet("notifications")]
        public async Task<IActionResult> AllNotifications()
        {
            var response = new ServiceResponses<IEnumerable<Notifications>>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();
                var user = await _context.Users.Include(i=> i.Notifications).FirstOrDefaultAsync(i => i.Id == userId);

                response.Messages = "Notification Retrieved for user";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = user.Notifications.OrderBy(i=> i.CreatedAt);


                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;

                return BadRequest(response);
            }

        }
        #endregion
        
        //Users Notifications for Read
        #region Read Notifications
        [HttpGet("readNotification")]
        public async Task<IActionResult> ReadNotifications()
        {
            var response = new ServiceResponses<IEnumerable<Notifications>>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);

                response.Messages = "Notification Retrieved for user";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = user.Notifications.Where(i=> i.Status == NotificationStatus.Read);


                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;

                return BadRequest(response);
            }

        }
        #endregion

        //Users Notifications for Read
        #region Unread Notifications
        [HttpGet("unreadNotification")]
        public async Task<IActionResult> UnReadNotifications()
        {
            var response = new ServiceResponses<IEnumerable<Notifications>>();
            try
            {
                string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                if (!int.TryParse(rawUser, out int userId))
                    return Unauthorized();
                var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == userId);

                response.Messages = "Notification Retrieved for user";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = user.Notifications.Where(i=> i.Status == NotificationStatus.UnRead);


                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = e.Message;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;

                return BadRequest(response);
            }

        }
        #endregion

        //Change notification status
        #region Change Notification Status
        [HttpPost("notification")]
        public async Task<IActionResult> ChangeStaus([FromBody]NotificationRequest request)
        {
            try
            {
                var notification = await _context.Notifications.FirstOrDefaultAsync(i => i.Id == request.Id);
                notification.Status = NotificationStatus.Read;
                return Ok("Successful");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}