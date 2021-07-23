using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Helpers
{
    public static class TaskResultExtensions
    {
        static public TaskResult FromException(this TaskResult _taskResult, Exception exception)
        {
            return new TaskResult
            {
                Failed = true,
                Exception = exception
            };
        }

    }
}
