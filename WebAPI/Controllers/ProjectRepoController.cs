using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.Service;
using Entities.Models;
using System.Diagnostics;
using Entities.Helpers;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class ProjectRepoController : ControllerBase
    {
        //IBlobFileService _blobFileService;
        IProjectRepoService _projectRepoService;
        ILogger<ProjectRepoController> _logger;
        public ProjectRepoController(IProjectRepoService projectRepoService, ILogger<ProjectRepoController> logger)
        {
            //_blobFileService = blobFileService;
            _projectRepoService = projectRepoService;
            _logger = logger;
        }

        //HttpGet
        [HttpGet("{projectId}/repo/ListAll")]
        public async Task<IActionResult> ListRepositoryFiles(string projectId)
        {
            var _result = await _projectRepoService.ListAll(projectId);
            return Ok(_result);
        }

        [HttpGet("{projectId}/repo/GetAll")]
        public async Task<IActionResult> GetRepositoryFiles(string projectId)
        {
            return null;
        }

        [HttpGet("{projectId}/repo/GetInfo")]
        public async Task<IActionResult> GetFileInfo(string projectId, string filePath)
        {
            return null;
        }

        [HttpGet("{projectId}/repo/{filePath}")]
        public async Task<IActionResult> DownloadFile(string projectId)
        {
            return null;
        }

        //HttpPost
        [HttpPost("{projectId}/repo/Upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(string projectId, [FromForm] ProjectFile projectFile)
        {
            try
            {
                _logger.LogTrace("Here");
                var _taskResult = await _projectRepoService.UploadFileAsync(projectId, projectFile);
                if (_taskResult.Failed)
                    return StatusCode(500, $"Internal Server Error. Message: {_taskResult.Exception.Message}");
                return Created("", projectFile);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
            
        }

        //HttpDelete
        [HttpDelete("{projectId}/repo/Delete/{filePath}")]
        public async Task<IActionResult> DeleteFile(string projectId, string filePath)
        {
            return null;
        }
    }
}
