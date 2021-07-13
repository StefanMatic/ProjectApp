using System;
using System.Collections.Generic;
using System.Text;
using Interfaces.Service;
using Entities.Entities;
using Entities.Models;

namespace Service.Tool
{
    class WorkItemMapper : IMapper<WorkItem,WorkItemEntity>
    {
       public WorkItem MapObject(WorkItemEntity _entity)
        {
            return new WorkItem
            {
                Id = _entity.PartitionKey,
                ProjectId = _entity.RowKey,
                Name = _entity.Name,
                Description = _entity.Description,
                State = _entity.State,
                OwningAliasId = _entity.OwningAliasId
            };
        }
        public WorkItemEntity MapEntity(WorkItem _object)
        {
            return new WorkItemEntity
            {
                PartitionKey = _object.Id,
                RowKey = _object.ProjectId,
                Name = _object.Name,
                Description = _object.Description,
                State = _object.State,
                OwningAliasId = _object.OwningAliasId
            };
        }
    }
}
