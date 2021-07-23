using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Entities.Models
{
    public class ProjectFile : ProjectFileBase
    {
        public DateTime ModifiedAt { get; private set; }
        public string ModifiedBy { get; set; }
        public string FullPath { get; set; }

        public IFormFile File { get; set; }
    }
}
