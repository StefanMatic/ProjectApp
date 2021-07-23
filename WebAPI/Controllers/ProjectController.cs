using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interfaces.Service;
using Interfaces.DAL;
using Entities.Models;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/")]
    public class ProjectController : ControllerBase
    {

        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            
        }

       [HttpPost("Project")]
       public async Task<IActionResult> CreateProject([FromBody] Project _project)
        {
            try
            {
                await _projectService.CreateAsync(_project);
                return Created("", _project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}" );
            }
        }
        /// HTTPGET
        [HttpGet("Projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                return Ok(await _projectService.ReadAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }
        
        [HttpGet("Projects/{id}")]
        public async Task<IActionResult> GetOneProject(string id)
        {
            var _result = await _projectService.ReadOneAsync(id);
            return Ok(_result);
        }

        /// HTTPPUT
        [HttpPut("Project/id")]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] Project _project)
        {
            try
            {
                await _projectService.UpdateAsync(_project);
                return Ok(_project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }

        [HttpDelete("Project/id")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            try
            {
                await _projectService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }

    }
}
