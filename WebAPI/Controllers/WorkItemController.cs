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
    public class WorkItemController : ControllerBase
    {

        private readonly IWorkItemService _workItemService;
        public WorkItemController(IWorkItemService workItemService)
        {
            _workItemService = workItemService ?? throw new ArgumentNullException(nameof(workItemService)); ;
        }

        [HttpPost("WorkItem")]
        public async Task<IActionResult> CreateWorkItem([FromBody] WorkItem _workItem)
        {
            try
            {
                await _workItemService.CreateAsync(_workItem);
                return Created("", _workItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }
        /// HTTPGET
        [HttpGet("WorkItems")]
        public async Task<IActionResult> GetAllWorkItems()
        {
            return Ok(await _workItemService.ReadAllAsync());
        }

        [HttpGet("WorkItems/{id}")]
        public async Task<IActionResult> GetOneWorkItem(string id)
        {
            var _result = await _workItemService.ReadOneAsync(id);
            return Ok(_result);
        }

        /// HTTPPUT
        [HttpPut("WorkItem/{id}")]
        public async Task<IActionResult> UpdateWorkItem(string id, [FromBody] WorkItem _workItem)
        {
            try
            {
                await _workItemService.UpdateAsync(_workItem);
                return Ok(_workItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }

        [HttpDelete("WorkItem/{id}")]
        public async Task<IActionResult> DeleteWorkItem(string id)
        {
            try
            {
                await _workItemService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error. Message: {ex.Message}");
            }
        }

    }
}
