using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class WorkItem
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
    }
}
