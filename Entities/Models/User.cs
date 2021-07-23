using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace Entities.Models
{
    public class User : UserBase
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
