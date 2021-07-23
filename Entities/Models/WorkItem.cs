﻿using System;
using System.Collections.Generic;
using System.Text;
using Entities.Enumerable;
using System.ComponentModel;

namespace Entities.Models
{
    public class WorkItem : BaseModel
    {

        [ReadOnly(true)]
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public Guid OwningAliasId { get; set; }
    }
}
