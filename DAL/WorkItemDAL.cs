using System;
using DAL.Azure;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using Interfaces.DAL;
using Entities.Entities;
using System.Threading.Tasks;

namespace DAL
{
    public class WorkItemDAL : IWorkItemDAL
    {
        const string _tableName = "WorkItem";

        private CloudTable _cloudTable;

        public WorkItemDAL()
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
        private async Task<bool> Exists(WorkItemEntity _entity)
        {
            TableOperation _tableOperation = TableOperation.Retrieve(_entity.PartitionKey, _entity.RowKey);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            if (_tableResult.HttpStatusCode == 404)
                return false;
            else
                return true;
        }
        public async Task<IEnumerable<WorkItemEntity>> ReadAllAsync()
        {
            List<WorkItemEntity> _projectEntities = new List<WorkItemEntity>();
            try
            {
                TableQuerySegment<WorkItemEntity> _querySeqment = null;
                var _tableQuery = new TableQuery<WorkItemEntity>();

                while (_querySeqment == null || _querySeqment.ContinuationToken != null)
                {
                    _querySeqment = await _cloudTable.ExecuteQuerySegmentedAsync<WorkItemEntity>(_tableQuery, _querySeqment != null ? _querySeqment.ContinuationToken : null);
                    _projectEntities.AddRange(_querySeqment.Results);
                }

                return _projectEntities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<WorkItemEntity> ReadOneAsync(string projectId)
        {
            try
            {
                TableQuerySegment<WorkItemEntity> _querySegment = null;
                var _tableQuery = new TableQuery<WorkItemEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, projectId));
                _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<WorkItemEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                if (_querySegment.Results.Count == 0)
                    throw new Exception("Project not found");
                return _querySegment.Results[0];

                //TableOperation _tableOperation = TableOperation.Retrieve<WorkItemEntity>(partitionKey,rowKey);
                //TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
                //if (_tableResult.HttpStatusCode == 404) throw new Exception();
                //var _entity = _tableResult.Result as WorkItemEntity;
                //if (_entity.Deleted) throw new Exception("Resource deleted;");
                //return _entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateAsync(WorkItemEntity _entity)
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
        public async Task UpdateAsync(WorkItemEntity _entity)
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
        public async Task DeleteAsync(WorkItemEntity _entity)
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
