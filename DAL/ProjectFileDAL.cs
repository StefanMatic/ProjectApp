using System;
using DAL.Connection;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using Interfaces.DAL;
using Entities.Entities;
using System.Threading.Tasks;

using Entities.Helpers;

namespace DAL
{
    public class ProjectFileDAL : IProjectFileDAL
    {
        private const string _tableName = "ProjectFiles";

        private CloudTable _cloudTable;
        public ProjectFileDAL(CloudStorageAccount _storageAccount)
        {
            try
            {
                var _tableClient = _storageAccount.CreateCloudTableClient();
                _cloudTable = _tableClient.GetTableReference(_tableName);
                _cloudTable.CreateIfNotExistsAsync();
            }
            catch (StorageException ex)
            {
                throw;
            }
        }


        //Get all project files by projectId
        public async Task<IEnumerable<ProjectFileEntity>> ReadAllAsync(string repositoryId)
        {

            List<ProjectFileEntity> _entities = new List<ProjectFileEntity>();
            try
            {
                TableQuerySegment<ProjectFileEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectFileEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, repositoryId));


                while (_querySegment == null || _querySegment.ContinuationToken != null)
                {
                    _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectFileEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                    _entities.AddRange(_querySegment.Results);
                }
                if (_entities.Count != 0)
                    return _entities;
                throw new Exception("No files");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //File name != full file path in storage
        public async Task<ProjectFileEntity> ReadOneAsync(string repositoryId, string fileName)
        {
            try
            {
                TableQuerySegment<ProjectFileEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectFileEntity>();
                //Adding query to filter by projectId and fileName
                _tableQuery.Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, repositoryId),"AND", TableQuery.GenerateFilterCondition("FileName", QueryComparisons.Equal, fileName)));
                _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectFileEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                if (_querySegment.Results.Count == 0)
                    throw new Exception("File not found");
                return _querySegment.Results[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TaskResult> CreateAsync(ProjectFileEntity _entity)
        {
            try
            {
                TableOperation _tableOperation = TableOperation.Insert(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
                return new TaskResult();
            }
            catch (Exception ex)
            {
                return TaskResult.FromException(ex);
            }
        }

        public async Task UpdateAsync(ProjectFileEntity _entity)
        {
            _entity.ETag = "*";
            try
            {
                TableOperation _tableOperation = TableOperation.InsertOrMerge(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task DeleteAsync(ProjectFileEntity _entity)
        {
            try
            {
                if (await Exists(_entity))
                {
                    TableOperation _tableOperation = TableOperation.Delete(_entity);
                    await _cloudTable.ExecuteAsync(_tableOperation);
                }
                else
                    throw new Exception("File Doesn't Exist");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Check to see if entity exists
        public async Task<bool> Exists(ProjectFileEntity _entity)
        {
            //Try to retrieve entity, return false if status code is 404 (Not found)
            try
            {
                TableOperation _tableOperation = TableOperation.Retrieve(_entity.PartitionKey, _entity.RowKey);
                TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
                if (_tableResult.HttpStatusCode == 404)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
