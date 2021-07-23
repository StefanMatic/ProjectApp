using System;
using System.Collections.Generic;
using System.Text;
using PhoneApp.Enumerables;

namespace PhoneApp.Models
{
    public class WorkItem : BaseModel
    {
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public Guid OwningAliasId { get; set; }
    }
}
