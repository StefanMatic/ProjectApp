using System;
using DAL.Connection;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using Interfaces.DAL;
using Entities.Entities;
using System.Threading.Tasks;
using System.Linq;

namespace DAL
{
    public class ProjectDAL : IProjectDAL
    {

        //Setting table name to be used by CloudTable 
        private const string _tableName = "Project";
        private CloudTable _cloudTable;


        public ProjectDAL(CloudStorageAccount _storageAccount)
        {
            //Try to create a table client and cloud table from storage account
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

        //Check to see if entity exists
        private async Task<bool> Exists(ProjectEntity _entity)
        {
            //Try to retrieve entity, return false if status code is 404 (Not found)
            try
            {
                TableOperation _tableOperation = TableOperation.Retrieve(_entity.PartitionKey,_entity.RowKey);
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

        //Not used here
        public async Task<IEnumerable<ProjectEntity>> ReadAllAsync()
        {
            List<ProjectEntity> _projectEntities = new List<ProjectEntity>();
            try
            {
                //Creating table query to query all project entities in table
                TableQuerySegment<ProjectEntity> _querySeqment = null;
                var _tableQuery = new TableQuery<ProjectEntity>();
                //While query segment is null (empty query result) and continuation token not null (next result not empty) read from table and add to list
                while (_querySeqment == null || _querySeqment.ContinuationToken != null) 
                {
                    _querySeqment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectEntity>(_tableQuery, _querySeqment != null ? _querySeqment.ContinuationToken : null);
                    _projectEntities.AddRange(_querySeqment.Results);
                }

                return _projectEntities;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Get all projects by projectId
        public async Task<IEnumerable<ProjectEntity>> ReadById(string id)
        {

            List<ProjectEntity> _entities = new List<ProjectEntity>();
            try
            {
                TableQuerySegment<ProjectEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));


                while (_querySegment == null || _querySegment.ContinuationToken != null)
                {
                    _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                    _entities.AddRange(_querySegment.Results);
                }
                if(_entities.Count != 0)
                    return _entities;
                throw new Exception("Project not found");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ProjectEntity> ReadOneAsync(string projectId)
        {
            try
            {
                TableQuerySegment<ProjectEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, projectId));
                _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                if(_querySegment.Results.Count == 0)
                    throw new Exception("Project not found");
                return _querySegment.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateAsync(ProjectEntity _entity)
        {
            try
            {
                if (!await Exists(_entity))
                {
                    TableOperation _tableOperation = TableOperation.Insert(_entity);
                    await _cloudTable.ExecuteAsync(_tableOperation);
                }
                else
                    throw new Exception("Project Exists");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task UpdateAsync(ProjectEntity _entity)
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
        public async Task DeleteAsync(ProjectEntity _entity)
        {
            try
            {
                if (await Exists(_entity))
                {
                    TableOperation _tableOperation = TableOperation.Delete(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
                }
                else
                    throw new Exception("Project Doesn't Exist");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetHighest()
        {

            List<ProjectEntity> _entities = new List<ProjectEntity>();
            TableQuerySegment<ProjectEntity> _querySegment = null;
            int _partitionKey = 0;
            var _tableQuery = new TableQuery<ProjectEntity>();

            while (_querySegment == null || _querySegment.ContinuationToken != null)
            {
                _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                _entities.AddRange(_querySegment.Results);
            }
            if (_entities.Count != 0)
                int.TryParse(_entities .OrderByDescending(x => x.PartitionKey).FirstOrDefault().PartitionKey, out _partitionKey);

            return _partitionKey;
        }
    }
}
