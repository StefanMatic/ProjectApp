using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Entities.Models
{
    public class ProjectFileBase
    {

        [ReadOnly(true)]
        public Guid FileId { get; private set; }
        public string FileName { get; set; }
        public string Description { get; set; }

        [ReadOnly(true)]
        public DateTime UploadedAt { get; private set; }

        [ReadOnly(true)]
        public string UploadedBy { get; set; }
    }
}
