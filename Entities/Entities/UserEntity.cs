using System;
using System.Collections.Generic;
using System.Text;
using Entities.Enumerable;
using Entities;

namespace Entities.Entities
{
    public class UserEntity : UserBase
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
