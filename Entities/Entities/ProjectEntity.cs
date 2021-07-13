using System;
using Entities.Enumerable;

namespace Entities.Entities
{
    public class ProjectEntity : Entity
    {
        public string Description { get; set; }
        public Guid CodeId { get; set; }
        public ProjectState State { get; set; }
    }
}
