﻿using System;
using Entities.Enumerable;

namespace Entities.Entities
{
    public class ProjectEntity : Entity
    {
        public string Description { get; set; }
        public Guid RepoId { get; set; }
        public ProjectState State { get; set; }
    }
}
