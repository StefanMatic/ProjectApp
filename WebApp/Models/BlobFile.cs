using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WebApp.Models
{
    public class BlobFile
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public Stream FileContent { get; set; }

    }
}
