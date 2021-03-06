using Microsoft.AspNetCore.Mvc;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
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
        private readonly DataBaseContext _context;
        public WelcomeController(DataBaseContext context)
        {
            _context = context;
        }
        // GET: api/<WelcomeController>
        [HttpGet]
        public IActionResult Get()
        {

            return Ok(_context.NewInfo.ToList());
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
