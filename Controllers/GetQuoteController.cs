using GeoCoordinatePortable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SjxLogistics.Controllers.CodeGen;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetQuoteController : ControllerBase
    {
        [HttpPost]
        public IActionResult GetQuoteCalculation(QuoteRequest quoteRequest)
        {
            var response = new ServiceResponses<int>();
            CalculateCharges calculateCharges = new CalculateCharges();
            var sPodition = new GeoCoordinate(quoteRequest.sLat, quoteRequest.sLag);
            var ePodition = new GeoCoordinate(quoteRequest.eLat, quoteRequest.eLag);
            double distance = sPodition.GetDistanceTo(ePodition) * 0.001;
            int price = calculateCharges.CalculateCharge(distance);
            response.StatusCode = 200;
            response.Success = true;
            response.Messages = "Value is equal to " + price;
            response.Data = price;
            return Ok(response);
        }
    }
}
