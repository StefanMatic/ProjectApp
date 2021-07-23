using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;
using Entities.Models;
using System.Security.Cryptography;

namespace Service.Tool
{
    public static class UserExtensions
    {
        private static int _saltLength = 15;
        private static int _passwordLength = 30;
        public static UserEntity GeneratePasswordHash(this UserEntity _userEntity, string password)
        {
            var _salt = GenerateSalt(_saltLength);
            var _hash = GenerateHash(password, _salt, _passwordLength);
            _userEntity.Password = _hash;
            _userEntity.Salt = _salt;
            return _userEntity;
        }

        public static bool VerifyPassword(this UserEntity _userEntity, string password)
        {
            var _salt = _userEntity.Salt;
            var _hash = GenerateHash(password, _salt, _passwordLength);
            return _hash.SequenceEqual(_userEntity.Password);
        }

        public static User FromEntity(this User _user,UserEntity userEntity)
        {
            _user.FirstName = userEntity.FirstName;
            _user.LastName = userEntity.LastName;
            _user.UserName = userEntity.UserName;
            return _user;
        }
        public static UserEntity ToEntity(this User _user)
        {
            var _userEntity = new UserEntity();
            _userEntity.FirstName = _user.FirstName;
            _userEntity.LastName = _user.LastName;
            _userEntity.UserName = _user.UserName;
            _userEntity.GeneratePasswordHash(_user.Password);
            return _userEntity;
        }




        private static byte[] GenerateSalt(int _saltLength)
        {
            var _bytes = new byte[_saltLength];
            using (var _rng = new RNGCryptoServiceProvider())
            {
                _rng.GetBytes(_bytes);
            }
            return _bytes;
        }

        private static byte[] GenerateHash(string _password, byte[] _salt, int _passwordLength)
        {
            using (var _deriveBytes = new Rfc2898DeriveBytes(_password, _salt))
            {
                return _deriveBytes.GetBytes(_passwordLength);
            }
        }
    }
}
