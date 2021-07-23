using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.DAL;
using Service;
using Entities.Models;
using Entities.Enumerable;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/")]
    public class UserController : ControllerBase
    {
        private IUserDAL _userDAL;
        private UserService _userService;
        private ILogger<UserController> _logger;
        public UserController(IUserDAL userDAL, UserService userService, ILogger<UserController> logger)
        {
            _userDAL = userDAL;
            _userService = userService;
            _logger = logger;
        }

        [Authorize(Roles = "User")]
        [HttpGet("Users/{userName}")]
        public async Task<IActionResult> GetOneUser(string userName)
        {
            var _result = await _userDAL.ReadAsync(userName);
            return Ok(_result);
        }
        
        
        [AllowAnonymous]
        [HttpPost("User")]
        public async Task<IActionResult> CreateUser([FromBody] User _user)
        {
            try
            {
                await _userService.CreateUser(_user);
                return Created("", _user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }
        [AllowAnonymous]
        [HttpPost("Auth")]
        public async Task<IActionResult> AuthUser([FromBody] Auth auth)
        {
            try
            {
                var _user = await _userService.Authenticate(auth.UserName, auth.Password);
                return Ok(_user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }



        [Authorize(Roles = "User")]
        [HttpGet("Auth/Validate")]
        public IActionResult ValidateUser()
        {
            return Ok();
        }

    }
}
