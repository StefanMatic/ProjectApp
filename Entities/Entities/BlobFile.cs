using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Entities.Entities
{
    public class BlobFile
    {
        //FullName == blobPath + blobName
        public string FullName { get; set; }
        public Stream FileContent { get; set; }

        public string SnapshotId { get; set; }

    }
}
