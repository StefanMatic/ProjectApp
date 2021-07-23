using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    public class ProjectFileHistoryEntity : Entity
    {
        public DateTime UploadedAt { get; set; }
        public long UploadedBy { get; set; }
    }
}
