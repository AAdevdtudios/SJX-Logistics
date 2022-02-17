using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SjxLogistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WelcomeController : ControllerBase
    {
        // GET: api/<WelcomeController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Our service helps to assist business in fulfilling orders <br /> within our global region and helps expand your business < br /> with express delivery");
        }

        // GET api/<WelcomeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<WelcomeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<WelcomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WelcomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
