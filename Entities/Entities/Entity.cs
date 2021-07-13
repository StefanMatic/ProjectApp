using Microsoft.Azure.Cosmos.Table;

namespace Entities.Entities
{
    public abstract class Entity : TableEntity
    {
        public bool Deleted { get; set; } = false;

        public Entity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public Entity()
        {

        }

        public bool isDeleted()
        {
            return this.Deleted;
        }
    }
}
