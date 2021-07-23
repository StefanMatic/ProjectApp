using System;
using System.Collections.Generic;
using System.Text;
using Interfaces.Service;
using System.Threading.Tasks;
using Entities.Entities;
using Entities.Models;

namespace Service.Tool
{
    internal class ProjectMapper : IMapper<Project,ProjectEntity>
    {
        public Project MapObject(ProjectEntity _entity)
        {
            return new Project
            {
                Id = _entity.PartitionKey,
                Name = _entity.RowKey,
                Description = _entity.Description,
                State = _entity.State
            };
        }
        public ProjectEntity MapEntity(Project _object)
        {
            return new ProjectEntity
            {
                PartitionKey = _object.Id,
                RowKey = _object.Name,
                Description = _object.Description,
                State = _object.State
            };
        }

    }
}
