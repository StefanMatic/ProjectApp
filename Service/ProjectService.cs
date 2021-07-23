using System;
using Interfaces.Service;
using Interfaces.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Models;
using Entities.Entities;
using Microsoft.Extensions.Caching.Memory;
using Service.Tool;

namespace Service
{
    public class ProjectService : IProjectService
    {
        IProjectDAL _projectDAL;
        IProjectWorkItemsDAL _projectWorkItemsDAL;
        //IMemoryCache _memoryCache;

        public ProjectService(IProjectDAL projectDAL, IProjectWorkItemsDAL projectWorkItemsDAL)
        {
            this._projectWorkItemsDAL = projectWorkItemsDAL;
            this._projectDAL = projectDAL;
            //this._memoryCache = memoryCache;
        }
        
        public async Task<IEnumerable<Project>> ReadAllAsync()
        {
            List<Project> _projects = new List<Project>();
            try
            {
                var _entities = await _projectDAL.ReadAllAsync();
                foreach (var _entity in _entities)
                {
                    var _project = Mapper.Map<Project>(_entity);
                    _projects.Add(_project);
                    _project.WorkItems = await GetProjectWorkitems(_entity.PartitionKey);
                }
                return _projects;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public async Task<Project> ReadOneAsync(string primaryKey)
        {
            try
            {
                ProjectEntity _entity = await _projectDAL.ReadOneAsync(primaryKey);
                var _object = Mapper.Map<Project>(_entity);
                _object.WorkItems = await GetProjectWorkitems(_entity.PartitionKey);
                return _object;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Creates a new project
        public async Task CreateAsync(Project _object)
        {
            try
            {
                //Map object to table entity
                ProjectEntity _entity = Mapper.Map<ProjectEntity>(_object);
                var id = await _projectDAL.GetHighest();
                if (id == 0)
                {
                    //Set to base id: 00000
                    id = 10000;
                }
                id++;
                _entity.PartitionKey = id.ToString();
                _entity.RepoId = Guid.NewGuid();

                await _projectDAL.CreateAsync(_entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }
        }
        public async Task UpdateAsync(Project _object)
        {
             try
            {
                ProjectEntity _entity = Mapper.Map<ProjectEntity>(_object);
                await _projectDAL.UpdateAsync(_entity);
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
                var _entity = await _projectDAL.ReadOneAsync(id);
                await _projectDAL.DeleteAsync(_entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetProjectWorkitems(string id)
        {
            List<string> _workItems = new List<string>();
            var _entities = await _projectWorkItemsDAL.ReadById(id);
            foreach(var _entity in _entities)
            {
                _workItems.Add(_entity.RowKey);
            }
            return _workItems;
            
        }
    }
}
