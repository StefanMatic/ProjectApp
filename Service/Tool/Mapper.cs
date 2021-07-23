using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;
using Entities.Models;
using Interfaces.Service;
using Microsoft.Extensions.Logging;

namespace Service.Tool
{
    public static class Mapper //: IMapper
    {
        
        public static Project Map<T>(ProjectEntity _entity) where T : Project
        {
            return new Project
            {
                Id = _entity.PartitionKey,
                Name = _entity.RowKey,
                Description = _entity.Description,
                State = _entity.State
            };
        }
        public static ProjectEntity Map<T>(Project _object) where T : ProjectEntity
        {
            return new ProjectEntity
            {
                PartitionKey = _object.Id,
                RowKey = _object.Name,
                Description = _object.Description,
                State = _object.State
            };
        }

        public static WorkItem Map<T>(WorkItemEntity _entity) where T : WorkItem
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
        public static WorkItemEntity Map<T>(WorkItem _object) where T : WorkItemEntity
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

        public static ProjectFileEntity Map<T>(ProjectFile _object) where T : ProjectFileEntity
        {
            return new ProjectFileEntity
            {
                RowKey = _object.FileId.ToString(), //?? new Guid().ToString(),
                FileName = _object.FileName,
                Description = _object.Description,
                //UploadedAt = _object.UploadedAt,
                UploadedBy = _object.UploadedBy,
                //ModifiedAt = _object.ModifiedAt,
                ModifiedBy = _object.ModifiedBy,
                FullPath = _object.FullPath
            };
        }
    }
}
