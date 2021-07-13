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
        IMemoryCache _memoryCache;
        ProjectMapper _mapper = new ProjectMapper();

        public ProjectService(IProjectDAL projectDAL, IMemoryCache memoryCache)
        {
            this._projectDAL = projectDAL;
            this._memoryCache = memoryCache;
        }
        
        public async Task<IEnumerable<Project>> ReadAllAsync()
        {
            List<Project> _projects = new List<Project>();
            try
            {
                var _entities = await _projectDAL.ReadAllAsync();
                foreach (var _entity in _entities)
                {
                    var _project = _mapper.MapObject(_entity);
                    _projects.Add(_project);
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
                var _object = _mapper.MapObject(_entity);
                return _object;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateAsync(Project _object)
        {
            try
            {
                ProjectEntity _entity = _mapper.MapEntity(_object);
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
                ProjectEntity _entity = _mapper.MapEntity(_object);
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
    }
}
