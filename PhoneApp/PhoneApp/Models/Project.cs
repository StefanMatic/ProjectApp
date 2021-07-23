using System;
using System.Collections.Generic;
using System.Text;
using PhoneApp.Enumerables;

namespace PhoneApp.Models
{
    public class Project : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectState State { get; set; }
        public List<string> WorkItems { get; set; }
    }
}
