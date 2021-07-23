using System;

namespace Entities.Helpers
{
    public class TaskResult
    {
        public TaskResult()
        {
            Failed = false;
        }
        public bool Failed { get; set; }
        public Exception Exception { get; set; }

        

        static public TaskResult FromException(Exception exception)
        {
            return new TaskResult
            {
                Failed = true,
                Exception = exception
            };
        }


    }
}
