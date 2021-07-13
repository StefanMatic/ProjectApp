using Microsoft.Azure.Cosmos.Table;

namespace AzureDataAccess
{
    sealed public class AzureStorageConnection
    {
        private static CloudStorageAccount _instance;

        public static CloudStorageAccount GetStorageAccount()
        {
            if (_instance == null)
            {
                _instance = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=projectappstorageaccount;AccountKey=zDgxdx5BFDgXF+PWFL7PpfosDX7y8V4eKS9POcq+EP21ih3MYSjs6D3wbdL/GnGmCSPvxh3nIQm5Am6gZ/7yFQ==;EndpointSuffix=core.windows.net");
            }

            return _instance;
        }

        public static CloudTable CloudTable(string tableName)
        {
            var _tableClient = GetStorageAccount().CreateCloudTableClient(new TableClientConfiguration());
            var _cloudTable = _tableClient.GetTableReference(tableName);
            return _cloudTable;
        }
    }
}
