using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CodeId { get; set; }
        public long TaskId { get; set; }
    }
}
