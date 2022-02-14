using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.Responses;
using SjxLogistics.Models.StaticClasses;
using System.Linq;

namespace SjxLogistics.Controllers.Report
{
    [Authorize(Roles = "Admin,Front-Desk,BackEnd")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public ReportController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("assigned")]
        public ActionResult<Riders> GetAssignedOrders()
        {
            var response = new ServiceResponses<IQueryable<Order>>();

            var order = _context.Order.Where(i => i.Status == OrderStatus.Assigned);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }


        [HttpGet("unassigned")]
        public ActionResult<Riders> GetUnAssignedOrders()
        {
            var response = new ServiceResponses<IQueryable<Order>>();

            var order = _context.Order.Where(i => i.Status == OrderStatus.Pending);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }


        [HttpGet("uncompleted")]
        public ActionResult<Riders> GetUnCompletedOrders()
        {
            var response = new ServiceResponses<IQueryable<Order>>();

            var order = _context.Order.Where(i => i.Status != OrderStatus.Delivered);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }


        [HttpGet("completed")]
        public ActionResult<Riders> GetCompletedOrders()
        {
            var response = new ServiceResponses<IQueryable<Order>>();

            var order = _context.Order.Where(i => i.Status == OrderStatus.Delivered);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }


        [HttpGet("uncompletedDelivery")]
        public ActionResult<Riders> GetUnCompletedDelivery()
        {
            var response = new ServiceResponses<IQueryable<Order>>();

            var order = _context.Order.Where(i => i.Status == OrderStatus.Transit);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }

        [HttpGet("canceledOrder")]
        public ActionResult<Riders> CanceledOrder()
        {
            var response = new ServiceResponses<IQueryable<Order>>();

            var order = _context.Order.Where(i => i.Status == OrderStatus.Canceled);
            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Data = order;
            response.Success = true;
            return Ok(response);
        }
    }
}
