using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Service;
using Interfaces.DAL;
using Service.Tool;
using Entities.Models;
using Entities.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Service
{
    public class WorkItemService : IWorkItemService
    {
        IWorkItemDAL _workItemDAL;
        IMemoryCache _memoryCache;
        IMapper<WorkItem,WorkItemEntity> _mapper = new WorkItemMapper();

        public WorkItemService(IWorkItemDAL workItemDAL, IMemoryCache memoryCache)
        {
            this._workItemDAL = workItemDAL;
            this._memoryCache = memoryCache;
        }

        public async Task<IEnumerable<WorkItem>> ReadAllAsync()
        {
            List<WorkItem> _workItems = new List<WorkItem>();
            try
            {
                var _entities = await _workItemDAL.ReadAllAsync();
                foreach (var _entity in _entities)
                {
                    var _object = _mapper.MapObject(_entity);
                    _workItems.Add(_object);
                }
                return _workItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<WorkItem> ReadOneAsync(string primaryKey)
        {
            try
            {
                WorkItemEntity _entity = await _workItemDAL.ReadOneAsync(primaryKey);
                var _object = _mapper.MapObject(_entity);
                return _object;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateAsync(WorkItem _object)
        {
            try
            {
                WorkItemEntity _entity = _mapper.MapEntity(_object);
                await _workItemDAL.CreateAsync(_entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
        public async Task UpdateAsync(WorkItem _object)
        {
            try
            {
                WorkItemEntity _entity = _mapper.MapEntity(_object);
                await _workItemDAL.UpdateAsync(_entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(string id)
        {
            try
            {
                var _entity = await _workItemDAL.ReadOneAsync(id);
                await _workItemDAL.DeleteAsync(_entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

