using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using SjxLogistics.Models.StaticClasses;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public OrdersController(DataBaseContext context)
        {
            _context = context;
        }
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<Order>> GetOrder()
        {
            var response = new ServiceResponses<IQueryable<Order>>();
            string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!int.TryParse(rawUser, out int userId))
                return Unauthorized();
            var user = await _context.Users.FindAsync(userId);
            var order = _context.Order.Where(i => i.Users.Id == user.Id);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }


        #region Add Order Based on User id
        // POST: api/Orders
        // The id passed is users id
        [HttpPost("create")]
        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderRequest order)
        {

            var response = new ServiceResponses<Order>();
            string rawUser = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (!int.TryParse(rawUser, out int userId))
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);

            var rand = new Random();
            var uid = rand.Next(10000, 100000);
            string deliveryCode = GenerateDeliveryCode(rand.Next(100, 1000));
            string orderCode = GetnewId(uid);
            int price = CalculateCharges(12);
            /*var responce = PaymentsMethod(order.PaymentType);*/
            try
            {
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
                        //Express = order.IsExpressDelivery,
                        ReceiversPhone = order.ReceiversPhone,
                        Charges = 100,
                        DeliveryCode = deliveryCode,
                        PaymentType = "Card",
                        CreatedAt = DateTime.Now.Date,
                    };
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
        #region Get via Order Code
        //Search for status of orders
        [HttpGet("[action]")]
        public async Task<ActionResult<Order>> OrderCode(string orderCode)
        {
            var response = new ServiceResponses<Order>();
            var order = await _context.Order.FirstOrDefaultAsync(i => i.OrderCode == orderCode);

            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }
        #endregion

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
    }
    #endregion
}
