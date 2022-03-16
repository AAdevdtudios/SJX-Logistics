using Microsoft.AspNetCore.Mvc;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.StaticClasses;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers.RiderController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignController : ControllerBase
    {
        private readonly DataBaseContext _context;
        public AssignController(DataBaseContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AssignOrder([FromBody] AssignRequest assign)
        {
            var order = await _context.Order.FindAsync(assign.OrderId);
            order.Status = OrderStatus.Assigned;
            var rider = await _context.Riders.FindAsync(assign.RiderOrder);
            try
            {
                rider.AssignedOrders.Add(order);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
