using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;
using Entities.Helpers;
using System.IO;

namespace Interfaces.DAL
{
    public interface IFileDAL<T>
    {
        void SetContainer(string containerName);

        Task<IEnumerable<string>> ListAll(string folderName);

        Task<TaskResult> UploadFileAsync(T blobFile);

        Task<T> DownloadFileAsync(string blobPath);
        Task<TaskResult> DeleteFileAsync(string blobPath);

        Task<TaskResult> CreateSnapshotAsync(string blobPath);

        Task<TaskResult> RestoreSnapshotAsync(string blobPath, string snapshotId);
        Task<TaskResult> DeleteSnapshotAsync(string blobPath, string snapshotId);
    }
}
