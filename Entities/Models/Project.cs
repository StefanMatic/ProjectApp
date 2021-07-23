using System;
using System.Collections.Generic;
using System.Text;
using Entities.Enumerable;
using System.ComponentModel;

namespace Entities.Models
{
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectState State { get; set; }

        [ReadOnly(true)]
        public List<string> WorkItems { get; set; }
    }
}
