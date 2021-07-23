using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Enumerable;

namespace WebApp.Models
{
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectState State { get; set; }
        public List<string> WorkItems { get; set; }
    }
}
