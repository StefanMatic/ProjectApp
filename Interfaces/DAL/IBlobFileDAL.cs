using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Interfaces.DAL
{
    public interface IBlobFileDAL : IFileDAL<BlobFile>
    {
        Task<bool> Exists(string blobPath);
    }
}
