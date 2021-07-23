using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.DAL;
using Entities.Entities;
using Microsoft.Data.SqlClient;
using DAL.Tools;

namespace DAL
{
    public class UserDAL : IUserDAL
    {
        private SqlConnection _sqlConnection;
        public UserDAL(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<UserEntity> ReadAsync(string userName)
        {
            var _sqlQuery = "SELECT * FROM Users Where UserName = @username";
            var _sqlCommand = new SqlCommand(_sqlQuery, _sqlConnection);
            _sqlCommand.Parameters.AddWithValue("username", userName);
            try
            {
                await _sqlConnection.OpenAsync();
                using(var _reader = await _sqlCommand.ExecuteReaderAsync())
                {
                    await _reader.ReadAsync();
                    return new UserEntity().GetFromReader(_reader);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateAsync(UserEntity entity)
        {
            var _sqlQuery = "INSERT INTO Users(FirstName,LastName,UserName,Password,Salt) VALUES (@firstName, @lastName, @username, @password, @salt)";
            var _sqlCommand = new SqlCommand(_sqlQuery, _sqlConnection);
            _sqlCommand.Parameters.AddWithValue("firstName", entity.FirstName);
            _sqlCommand.Parameters.AddWithValue("lastName", entity.LastName);
            _sqlCommand.Parameters.AddWithValue("userName", entity.UserName);
            _sqlCommand.Parameters.AddWithValue("password", entity.Password);
            _sqlCommand.Parameters.AddWithValue("salt", entity.Salt);
            try
            {
                await _sqlConnection.OpenAsync();
                await _sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task UpdateAsync(UserEntity entity)
        {

        }
        public async Task DeleteAsync(UserEntity entity)
        {

        }
    }
}
