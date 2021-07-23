using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Entities.Models
{
    public class BaseModel
    {

        [ReadOnly(true)]
        public string Id { get; set; }
    }
}
