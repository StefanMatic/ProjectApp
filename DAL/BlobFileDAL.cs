using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DAL.Connection;
using System.Threading.Tasks;
using System.IO;
using Entities.Entities;
using Entities.Helpers;
using Interfaces.DAL;

namespace DAL
{
    public class BlobFileDAL : IBlobFileDAL
    {
        private BlobServiceClient _blobServiceClient;
        private BlobContainerClient _blobContainerClient;

        public BlobFileDAL(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public void SetContainer(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<IEnumerable<string>> ListAll(string folderName)
        {
            try
            {
                List<string> _files = new List<string>();
                await foreach (var _blob in _blobContainerClient.GetBlobsAsync(prefix: folderName))
                {
                    _files.Add(_blob.Name.Remove(0,folderName.Length+1));
                }
                return _files;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<BlobFile> GetFileInfo(string blobPath)
        //{
        //    var _blobs = _blobContainerClient.GetBlobsAsync(prefix: blobPath);
        //    var _blobClient = _blobContainerClient.GetBlobClient(blobPath);
        //    return new BlobFile();
        //}


        // blobPath == fullFilePath
        public async Task<TaskResult> UploadFileAsync(BlobFile blobFile)
        {
            try
            {
                await _blobContainerClient.UploadBlobAsync(blobFile.FullName, blobFile.FileContent);
                return new TaskResult();
            }
            catch(Exception ex)
            {
                return TaskResult.FromException(ex);
            }
        }

        public async Task<BlobFile> DownloadFileAsync(string blobPath)
        {
            var _blobClient = _blobContainerClient.GetBlobClient(blobPath);
            Stream _stream = null;
            await _blobClient.DownloadToAsync(_stream);
            return null;
        }

        public async Task<TaskResult> DeleteFileAsync(string blobPath)
        {
            return null;
        }

        public async Task<TaskResult> CreateSnapshotAsync(string blobPath)
        {
            return null;
        }

        public async Task<TaskResult> RestoreSnapshotAsync(string blobPath, string snapshotId)
        {
            return null;
        }
        public async Task<TaskResult> DeleteSnapshotAsync(string blobPath, string snapshotId)
        {
            return null;
        }

        public async Task<bool> Exists(string blobPath)
        {
            var _blobClient = _blobContainerClient.GetBlobClient(blobPath);
            return await _blobClient.ExistsAsync();
        }
        
    }
}
