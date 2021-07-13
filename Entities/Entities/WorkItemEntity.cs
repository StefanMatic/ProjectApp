using System;
using Entities.Enumerable;

namespace Entities.Entities
{
    public class WorkItemEntity : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public Guid OwningAliasId { get; set; }
    }
}
