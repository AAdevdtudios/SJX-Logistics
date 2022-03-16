using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SjxLogistics.Components;
using SjxLogistics.Controllers.AuthenticationComponent;
using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using SjxLogistics.Models.Request;
using SjxLogistics.Models.Responses;
using SjxLogistics.Models.StaticClasses;
using SjxLogistics.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IpasswordHasher _ipasswordHasher;
        private readonly DataBaseContext _context;
        private readonly IMailService _mailService;
        private readonly AccessToken _accessTokkenGenerator;
        public AuthenticationController(IpasswordHasher ipasswordHasher, DataBaseContext context, AccessToken accessToken, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
            _ipasswordHasher = ipasswordHasher;
            _accessTokkenGenerator = accessToken;
        }


        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request)
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

            Users usersEmail = _context.Users.FirstOrDefault(i => i.Email == request.Email || i.PhoneNumber == request.PhoneNumber);
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
                Users requestUser = new()
                {
                    Email = request.Email,
                    Password = passwordHash,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Role = Roles.User,
                    Address = request.Address
                };
                string token = _accessTokkenGenerator.GenerateToken(requestUser);
                await _context.Users.AddAsync(requestUser);
                await _context.SaveChangesAsync();
                response.Messages = "Successful";
                response.StatusCode = 200;
                response.Success = true;
                response.Data = requestUser;
                response.Token = token;

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
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
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
            /*if(request.PhoneNumber != null)
            {
                Users userInfo = await _context.Users.Include(i => i.Orders).FirstOrDefaultAsync(i => i.Email == request.Email);
            }*/
            Users userInfo = await _context.Users.Include(i => i.Orders).FirstOrDefaultAsync(i => i.Email == request.Email);
            if (userInfo == null)
            {
                response.Messages = "User does not exist";
                response.StatusCode = 401;
                response.Success = false;
                response.Data = null;

                return Unauthorized(response);
            }
            bool isCorrect = _ipasswordHasher.VerifyPassword(request.Password, userInfo.Password);
            if (!isCorrect)
            {
                response.Messages = "User password is Incorrect";
                response.StatusCode = 401;
                response.Success = false;
                response.Data = null;

                return Unauthorized(response);
            }
            string token = _accessTokkenGenerator.GenerateToken(userInfo);


            response.Messages = "Successful";
            response.StatusCode = 200;
            response.Success = true;
            response.Data = userInfo;
            response.Token = token;

            return Ok(response);
        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> GetUserByEmail([FromBody] EmailRequest request)
        {
            var response = new ServiceResponses<Users>();
            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            };
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.ToEmail);
            if (user == null)
            {
                response.Data = null;
                response.Success = false;
                response.Messages = "User does not exist";
                return BadRequest(response);
            }
            else
            {
                var tokken = _accessTokkenGenerator.GenerateResetPasswordToken(user);
                try
                {
                    request.Body = "";
                    await _mailService.SendEmailAsync(request);
                    response.Success = true;
                    response.Messages = "Data fetched Successfully";
                    response.Data = user;
                    response.Token = tokken;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }
            };

        }



        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers(string email)
        {
            var response = new ServiceResponses<Users>();
            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            };
            var user = await _context.Users.FindAsync(email);
            if (user == null)
            {
                response.Data = null;
                response.Success = false;
                response.Messages = "Un-authorize user";
                return Unauthorized(response);
            }
            else
            {
                var users = await _context.Users.Include(u => u.Orders).Include(u => u.Notifications).ToListAsync();

                if (users.Count > 0)
                {
                    response.Success = true;
                    response.Messages = "Data fetched Successfully";
                    response.Data = user;
                    return Ok(response);
                }
                else
                {
                    response.Success = false;
                    response.Messages = "No record found";
                    response.Data = null;
                    return NotFound(response);
                }

            };

        }





        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] Users model)
        {
            var response = new ServiceResponses<Users>();
            if (!ModelState.IsValid)
            {
                response.Messages = "One or more field is empty";
                response.StatusCode = 400;
                response.Success = false;
                response.Data = null;
                return BadRequest(response);
            };
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Email == model.Email);

            if (user == null)
            {
                response.Success = false;
                response.Messages = "User does not exist";
                return BadRequest(response);

            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            _context.Update(user);
            await _context.SaveChangesAsync();
            response.Success = true;
            response.Messages = "User updated Succesfully";
            response.Data = user;
            return Ok(response);
        }
    }
    }

