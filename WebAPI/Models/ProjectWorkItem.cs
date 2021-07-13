using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ProjectWorkItem
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
    }
}
