using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SjxLogistics.Components;
using SjxLogistics.Controllers.AuthenticationComponent;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using SjxLogistics.Models.StaticClasses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IpasswordHasher _ipasswordHasher;
        private readonly DataBaseContext _context;
        private readonly AccessToken _accessTokkenGenerator;

        public AdminController(IpasswordHasher ipasswordHasher, DataBaseContext context, AccessToken accessToken)
        {
            _context = context;
            _ipasswordHasher = ipasswordHasher;
            _accessTokkenGenerator = accessToken;
        }

        /*
         * Admin Responsibilities Create Admin 
         * Create Rider 
         * Create Front Desk
         * Create Back End
         */
        //[Authorize(Roles = "Users")]
        [HttpPost("admin/create")]
        public async Task<IActionResult> CreateAdmin([FromBody] RoleRegisterRequest request)
        {
            var response = new ServiceResponses<Users>();
            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

            Users usersEmail = _context.Users.FirstOrDefault(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }
            try
            {
                string passwordHash = _ipasswordHasher.HashPassword(request.Password);
                Users requestUser = new ()
                {
                    Email = request.Email,
                    Password = passwordHash,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Role = Roles.Admin,
                    Address = request.Address
                };
                await _context.Users.AddAsync(requestUser);
                await _context.SaveChangesAsync();
                string token = _accessTokkenGenerator.GenerateToken(requestUser);
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = requestUser;
                response.Tokken = token;

                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = "Failed" + e.Message;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }
        }
        [HttpPost("frontDesk/create")]
        public async Task<IActionResult> CreateFrontDesk([FromBody] RoleRegisterRequest request)
        {
            var response = new ServiceResponses<Users>();
            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

            Users usersEmail = _context.Users.FirstOrDefault(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }
            try
            {
                string passwordHash = _ipasswordHasher.HashPassword(request.Password);
                Users requestUser = new ()
                {
                    Email = request.Email,
                    Password = passwordHash,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Role = Roles.FrontDesk,
                    Address = request.Address
                };
                await _context.Users.AddAsync(requestUser);
                await _context.SaveChangesAsync();
                string token = _accessTokkenGenerator.GenerateToken(requestUser);
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = requestUser;
                response.Tokken = token;

                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = "Failed" + e.Message;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }
        }
        [Authorize(Roles = "Admin,Front-Desk")]
        [HttpPost("rider/create")]
        public async Task<IActionResult> CreateRider([FromBody] RoleRegisterRequest request)
        {
            var response = new ServiceResponses<Users>();
            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }

            Users usersEmail = _context.Users.FirstOrDefault(i => i.Email == request.Email);
            if (usersEmail != null)
            {
                response.Messages = "This user already exist";
                response.StatusCode = 409;
                response.Success = false;
                response.Data = null;

                return Conflict(response);
            }
            try
            {
                string passwordHash = _ipasswordHasher.HashPassword(request.Password);
                Riders requestUser = new ()
                {
                    Email = request.Email,
                    Password = passwordHash,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Role = Roles.Rider,
                    Address = request.Address
                };
                await _context.Users.AddAsync(requestUser);
                await _context.Riders.AddAsync(requestUser);
                await _context.SaveChangesAsync();
                string token = _accessTokkenGenerator.GenerateToken(requestUser);
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = requestUser;
                response.Tokken = token;

                return Ok(response);
            }
            catch (Exception e)
            {
                response.Messages = "Failed" + e.Message;
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            }
        }
    }
}
