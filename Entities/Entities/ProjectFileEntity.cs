using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Entities
{
    public class ProjectFileEntity : Entity
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public string FullPath
        {
            get => $"{PartitionKey}/{FullPath}";
            set => FullPath = value;
        }

    }
}
