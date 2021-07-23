using System;
using DAL.Connection;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using Interfaces.DAL;
using Entities.Entities;
using System.Threading.Tasks;

namespace DAL
{
    public class ProjectWorkItemsDAL : IProjectWorkItemsDAL
    {
        const string _tableName = "ProjectWorkItems";

        private CloudTable _cloudTable;

        public ProjectWorkItemsDAL()
        {
            try
            {
                _cloudTable = AzureStorageConnection.CloudTable(_tableName);
                _cloudTable.CreateIfNotExistsAsync();
            }
            catch (StorageException ex)
            {

                throw ex;
            }
        }
        private async Task<bool> Exists(ProjectWorkItemEntity _entity)
        {
            TableOperation _tableOperation = TableOperation.Retrieve(_entity.PartitionKey, _entity.RowKey);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            if (_tableResult.HttpStatusCode == 404)
                return false;
            else
                return true;
        }
        public async Task<IEnumerable<ProjectWorkItemEntity>> ReadAllAsync()
        {
            List<ProjectWorkItemEntity> _projectEntities = new List<ProjectWorkItemEntity>();
            try
            {
                TableQuerySegment<ProjectWorkItemEntity> _querySeqment = null;
                var _tableQuery = new TableQuery<ProjectWorkItemEntity>();

                while (_querySeqment == null || _querySeqment.ContinuationToken != null)
                {
                    _querySeqment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectWorkItemEntity>(_tableQuery, _querySeqment != null ? _querySeqment.ContinuationToken : null);
                    _projectEntities.AddRange(_querySeqment.Results);
                }

                return _projectEntities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<ProjectWorkItemEntity>> ReadById(string id)
        {

            List<ProjectWorkItemEntity> _entities = new List<ProjectWorkItemEntity>();
            try
            {
                TableQuerySegment<ProjectWorkItemEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectWorkItemEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));


                while (_querySegment == null || _querySegment.ContinuationToken != null)
                {
                    _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectWorkItemEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                    _entities.AddRange(_querySegment.Results);
                }return _entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProjectWorkItemEntity> ReadOneAsync(string projectId)
        {
            try
            {
                TableQuerySegment<ProjectWorkItemEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectWorkItemEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, projectId));
                _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectWorkItemEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                if (_querySegment.Results.Count == 0)
                    throw new Exception("Project not found");
                return _querySegment.Results[0];

                //TableOperation _tableOperation = TableOperation.Retrieve<ProjectWorkItemEntity>(partitionKey,rowKey);
                //TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
                //if (_tableResult.HttpStatusCode == 404) throw new Exception();
                //var _entity = _tableResult.Result as ProjectWorkItemEntity;
                //if (_entity.Deleted) throw new Exception("Resource deleted;");
                //return _entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateAsync(ProjectWorkItemEntity _entity)
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
                throw ex;
            }
        }
        public async Task UpdateAsync(ProjectWorkItemEntity _entity)
        {
            _entity.ETag = "*";
            try
            {
                TableOperation _tableOperation = TableOperation.InsertOrMerge(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task DeleteAsync(ProjectWorkItemEntity _entity)
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
                throw ex;
            }
        }
    }

}
