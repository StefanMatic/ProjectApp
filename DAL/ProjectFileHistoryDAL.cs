using System;
using DAL.Connection;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using Interfaces.DAL;
using Entities.Entities;
using System.Threading.Tasks;

namespace DAL
{
    public class ProjectFileHistoryDAL : IProjectFileHistoryDAL
    {
        private const string _tableName = "ProjectFileHistory";

        private CloudTable _cloudTable;
        public ProjectFileHistoryDAL(CloudStorageAccount _storageAccount)
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


        //Get all file history for a file
        public async Task<IEnumerable<ProjectFileHistoryEntity>> ReadAllAsync(string fileId)
        {

            List<ProjectFileHistoryEntity> _entities = new List<ProjectFileHistoryEntity>();
            try
            {
                TableQuerySegment<ProjectFileHistoryEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectFileHistoryEntity>();
                _tableQuery.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, fileId));


                while (_querySegment == null || _querySegment.ContinuationToken != null)
                {
                    _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectFileHistoryEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
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

        //Get single file history info by fileId and snapshotId
        public async Task<ProjectFileHistoryEntity> ReadOneAsync(string fileId, string snapshotId)
        {
            try
            {
                TableQuerySegment<ProjectFileHistoryEntity> _querySegment = null;
                var _tableQuery = new TableQuery<ProjectFileHistoryEntity>();
                //Adding query to filter by projectId and fileName
                _tableQuery.Where(TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, fileId), "AND", TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, snapshotId)));
                _querySegment = await _cloudTable.ExecuteQuerySegmentedAsync<ProjectFileHistoryEntity>(_tableQuery, _querySegment != null ? _querySegment.ContinuationToken : null);
                if (_querySegment.Results.Count == 0)
                    throw new Exception("File not found");
                return _querySegment.Results[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateAsync(ProjectFileHistoryEntity _entity)
        {
            try
            {
                if (!await Exists(_entity))
                {
                    TableOperation _tableOperation = TableOperation.Insert(_entity);
                    await _cloudTable.ExecuteAsync(_tableOperation);
                }
                else
                    throw new Exception("Snapshot Exists");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateAsync(ProjectFileHistoryEntity _entity)
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
        public async Task DeleteAsync(ProjectFileHistoryEntity _entity)
        {
            try
            {
                if (await Exists(_entity))
                {
                    TableOperation _tableOperation = TableOperation.Delete(_entity);
                    await _cloudTable.ExecuteAsync(_tableOperation);
                }
                else
                    throw new Exception("Snapshot Doesn't Exist");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Check to see if entity exists
        private async Task<bool> Exists(ProjectFileHistoryEntity _entity)
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
