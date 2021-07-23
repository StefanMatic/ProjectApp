using Microsoft.Azure.Cosmos.Table;
using Azure.Storage.Blobs;

namespace DAL.Connection
{
    sealed public class AzureStorageConnection
    {
        private static CloudStorageAccount _instance;
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=projectappstorageaccount;AccountKey=zDgxdx5BFDgXF+PWFL7PpfosDX7y8V4eKS9POcq+EP21ih3MYSjs6D3wbdL/GnGmCSPvxh3nIQm5Am6gZ/7yFQ==;EndpointSuffix=core.windows.net";

        public static CloudStorageAccount GetStorageAccount()
        {
            if (_instance == null)
            {
                _instance = CloudStorageAccount.Parse(_connectionString);
            }

            return _instance;
        }

        public static CloudTable CloudTable(string tableName)
        {
            var _tableClient = GetStorageAccount().CreateCloudTableClient(new TableClientConfiguration());
            var _cloudTable = _tableClient.GetTableReference(tableName);
            return _cloudTable;
        }

        public static BlobContainerClient GetClient(string containerName)
        {
            BlobContainerClient _blobContainerClient = new BlobContainerClient(_connectionString, containerName);
            return _blobContainerClient;
        }
    }
}
