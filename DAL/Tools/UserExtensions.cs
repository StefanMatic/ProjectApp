using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Tools
{
    public static class UserExtensions
    {
        public static UserEntity GetFromReader(this UserEntity _userEntity ,SqlDataReader _reader)
        {
            int index = 0;
            _userEntity.Id = _reader.GetInt32(index++);
            _userEntity.FirstName = _reader.GetString(index++);
            _userEntity.LastName = _reader.GetString(index++);
            _userEntity.UserName = _reader.GetString(index++);
            _userEntity.Password = _reader.GetSqlBytes(index++).Buffer;
            _userEntity.Salt = _reader.GetSqlBytes(index++).Buffer;
            _userEntity.Role = _reader.GetString(index++);
            return _userEntity;
        }
    }
}
