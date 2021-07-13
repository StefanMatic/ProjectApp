using Microsoft.Azure.Cosmos.Table;

namespace AzureDataAccess.Models
{
    public abstract class Entity : TableEntity
    {
        public bool Deleted { get; set; } = false;

    }
}
