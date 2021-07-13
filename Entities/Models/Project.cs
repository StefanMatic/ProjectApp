using System;
using System.Collections.Generic;
using System.Text;
using Entities.Enumerable;

namespace Entities.Models
{
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CodeId { get; set; }
        public ProjectState State { get; set; }
    }
}
