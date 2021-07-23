using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;
using Entities.Models;
using Entities.Helpers;

namespace Interfaces.Service
{
    public interface IProjectRepoService
    {
        Task<IEnumerable<string>> ListAll(string projectId);
        Task<TaskResult> UploadFileAsync(string projectId, ProjectFile projectFile);
    }
}
