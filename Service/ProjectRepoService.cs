using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Service;
using Interfaces.DAL;
using Entities.Models;
using Entities.Entities;
using Entities.Helpers;
using Service.Tool;

using Microsoft.Extensions.Logging;

namespace Service
{
    public class ProjectRepoService : IProjectRepoService
    {
        private IBlobFileDAL _blobFileDAL;
        private IProjectDAL _projectDAL;
        private IProjectFileDAL _projectFileDAL;
        private IProjectFileHistoryDAL _projectFileHistoryDAL;
        ILogger<ProjectRepoService> _logger;
        public ProjectRepoService(IBlobFileDAL blobFileDAL, IProjectDAL projectDAL, IProjectFileDAL projectFileDAL, IProjectFileHistoryDAL projectFileHistoryDAL, ILogger<ProjectRepoService> logger)
        {
            _projectDAL = projectDAL;
            _blobFileDAL = blobFileDAL;
            _blobFileDAL.SetContainer("projects");
            _projectFileDAL = projectFileDAL;
            _projectFileHistoryDAL = projectFileHistoryDAL;
            _logger = logger;
        }
        public async Task<IEnumerable<string>> ListAll(string projectId)
        {
            var _repoId = await GetRepo(projectId);
            return await _blobFileDAL.ListAll(_repoId);
        }

        //Upload file handles both creating a new file and updating the existing one with all additional information as file information and file history information
        public async Task<TaskResult> UploadFileAsync(string projectId, ProjectFile projectFile)
        {
            try
            {
                _logger.LogTrace("Starting upload");
                var _repoId = await GetRepo(projectId);
                //var fullPath = $"{projectId}/{file.FullPath}";
                _logger.LogTrace("Repository id");
                _logger.LogTrace(_repoId);

                ProjectFileEntity _projectFileEntity = new ProjectFileEntity { PartitionKey = projectId, RowKey = projectFile.FileId.ToString() };
                var _fullPath = $"{_repoId}/{projectFile.FullPath}";
                _logger.LogTrace("FullPath");
                _logger.LogTrace(_fullPath);
                bool _fileExists = await _blobFileDAL.Exists(_fullPath) | await _projectFileDAL.Exists(_projectFileEntity);
                _logger.LogTrace("File Exists?");
                _logger.LogTrace(_fileExists.ToString());
                if (_fileExists)
                {
                    //Update the existing file, add file history, upload to blob
                    return TaskResult.FromException(new Exception("File exists"));
                }
                else
                {
                    //Add new file, add new entry to ProjectFiles

                    //Map ProjectFile to ProjectFileEntity
                    _logger.LogTrace("Mapping");
                    _logger.LogTrace("Project parameters");
                    _logger.LogTrace($"{projectFile.FileId}\n{projectFile.FileName}");
                    _projectFileEntity = Mapper.Map<ProjectFileEntity>(projectFile);
                    _logger.LogTrace("Mapped");
                    _projectFileEntity.PartitionKey = _repoId;
                    _projectFileEntity.RowKey = projectFile.FileId.ToString() ?? Guid.NewGuid().ToString();
                    _projectFileEntity.UploadedAt = DateTime.UtcNow;
                    _logger.LogTrace($"Mapped projectfil to entity, repoId: {_repoId}");

                    //Get file stream from ProjectFile and map to BlobFile - BlobDAL
                    BlobFile _blobFile = new BlobFile
                    {
                        //FullName = _projectFileEntity.FullPath
                        FullName = _projectFileEntity.FileName
                    };
                    if(projectFile.File.Length <= 0)
                        return TaskResult.FromException(new Exception("Invalid file"));
                    await projectFile.File.CopyToAsync(_blobFile.FileContent);

                    //Upload file to blob and get result
                    var _taskResult = await _blobFileDAL.UploadFileAsync(_blobFile);
                    if (_taskResult.Failed)
                        return _taskResult;

                    //Add new entry to ProjectFile Table
                    _taskResult = await _projectFileDAL.CreateAsync(_projectFileEntity);
                    if (_taskResult.Failed)
                        return _taskResult;

                    //Return success
                    return new TaskResult();

                }
            }
            catch (Exception ex)
            {
                return TaskResult.FromException(ex);
            }
            //await _blobFileDAL.UploadFileAsync(fullPath, file.FileContent);
        }

        public async Task<BlobFile> DownloadFileAsync(string projectId, string blobPath)
        {
            var _repoId = await GetRepo(projectId);
            var fullPath = $"{projectId}/{blobPath}";

            return new BlobFile();
        }

        private async Task<string> GetRepo(string projectId)
        {
            var _project = await _projectDAL.ReadOneAsync(projectId);
            return _project.RepoId.ToString();
        }
    }
}
