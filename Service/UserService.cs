using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;
using Microsoft.Extensions.Options;
using Entities.Models;
using Service.Tool;
using Interfaces.DAL;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Diagnostics;

namespace Service
{
    public class UserService
    {
        private IUserDAL _userDAL;
        private IConfiguration Configuration;
        private ILogger<UserService> _logger;
        public UserService(IUserDAL userDAL, IConfiguration configuration, ILogger<UserService> logger)
        {
            Configuration = configuration;
            _userDAL = userDAL;
            _logger = logger;
        }
        public async Task<User> CreateUser(User _user)
        {
            try
            {
                await _userDAL.CreateAsync(_user.ToEntity());
                return _user;
            }
            catch
            {
                return null;
            }

        }

        public async Task<User> Authenticate(string username, string password)
        {
            //Gets user entity associated to the user 
            var _userEntity = await _userDAL.ReadAsync(username);
            if(!_userEntity.VerifyPassword(password)) //Verifies password using the Extension class
                return null;
            var _user = new User().FromEntity(_userEntity); //Gets user entity from Extension class

            var _keyString = Configuration["webapi-secretkey"]; //Grabs secret key from keyvault
            var _secretKey = Encoding.ASCII.GetBytes("rQWZdh7?w2cz243gpmJJJchxqtPcRcAG");
            var _tokenHandler = new JwtSecurityTokenHandler();
            var _tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, _userEntity.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")

                }),
                Expires = DateTime.UtcNow.AddDays(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var _token = _tokenHandler.CreateToken(_tokenDescriptor);
            _user.Token = _tokenHandler.WriteToken(_token);
            _logger.LogInformation($"Printing token for verificatiom om the input password {_user.Token}");
            return _user;

        }


    }
}
