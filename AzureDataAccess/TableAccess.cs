using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using AzureDataAccess.Models;

namespace AzureDataAccess
{
    public class TableAccess
    {
        private readonly CloudStorageAccount _storageAccount;

        private CloudTable _cloudTable;

        public TableAccess(string tableName)
        {
            try
            {
                _cloudTable = AzureStorageConnection.CloudTable(tableName);
            }
            catch(Exception ex)
            {
                
            }
        }

        

        public bool EntityExists(string partitionId, string rowId)
        {
            var _task = GetEntity<Entity>(partitionId, rowId);
            _task.Wait();
            var _result = _task.Result;
            return _result == null ? false : true;
        }

        public async Task<TableEntity> InsertOrMergeEntityAsync(TableEntity _entity)
        {
            TableOperation _tableOperation = TableOperation.InsertOrMerge(_entity);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            TableEntity _tableEntity = _tableResult.Result as TableEntity;
            return _tableEntity;
        }

        public async Task InsertDynamicEntity(DynamicTableEntity _entity)
        {
            try
            {
                TableOperation _tableOperation = TableOperation.Insert(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task InsertOrMergeDynamicEntity(DynamicTableEntity _entity)
        {
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

        public async Task<TableEntity> MergeEntityAsync(TableEntity _entity)
        {
            _entity.ETag = "*";
            TableOperation _tableOperation = TableOperation.Merge(_entity);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            return _tableResult.Result as TableEntity;
        }
        public async Task MergeDynamicEntityAsync(DynamicTableEntity _entity)
        {
            _entity.ETag = "*";
            try
            {
                TableOperation _tableOperation = TableOperation.Merge(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TableEntity> ReplaceEntityAsync(TableEntity _entity)
        {
            _entity.ETag = "*";
            TableOperation _tableOperation = TableOperation.Replace(_entity);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            return _tableResult.Result as TableEntity;
        }
        public async Task ReplaceDynamicEntityAsync(DynamicTableEntity _entity)
        {
            _entity.ETag = "*";
            try
            {
                TableOperation _tableOperation = TableOperation.Replace(_entity);
                await _cloudTable.ExecuteAsync(_tableOperation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DynamicTableEntity> GetDynamicEntity(string _partitionKey, string _rowKey)
        {
            TableOperation _tableOperation = TableOperation.Retrieve(_partitionKey, _rowKey);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            if (_tableResult.HttpStatusCode == 404) throw new Exception("Doesn't exist");
            DynamicTableEntity _entity = _tableResult.Result as DynamicTableEntity;
            return _entity;
        }


        public async Task<Entity> GetEntity<T>(string _partitionKey, string _rowKey) where T : Entity
        {
            TableOperation _tableOperation = TableOperation.Retrieve<T>(_partitionKey, _rowKey);
            TableResult _tableResult = await _cloudTable.ExecuteAsync(_tableOperation);
            if (_tableResult.HttpStatusCode == 404) throw new Exception("Doesn't exist");
            Entity _entity = _tableResult.Result as Entity;
            if (_entity.Deleted) throw new Exception("Resource deleted;");
            return _entity;
        }

        public async Task<T> GetEntitiesAsync<T>()
        {
            return (T)await Task.Run(() => GetEntities());
        }

        private IEnumerable<DynamicTableEntity> GetEntities()
        {
            TableQuery _tableQuery = new TableQuery();
            var _entities = _cloudTable.ExecuteQuery(_tableQuery) ;
            return _entities ;
        }
    }
}
