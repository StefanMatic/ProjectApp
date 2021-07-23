using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Service
{
    public interface IFileService<T>
    {
        Task<IEnumerable<string>> ListAll(string folderName);
        Task UploadFileAsync(string projectId, T file);

        Task<T> DownloadFileAsync(string blobPath, string fileName);
    }
}
