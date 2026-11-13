using System;

namespace Rocket.Integrations.Domain
{
    public class JobStatusEntry
    {
        public int ExecutionStatus { get; set; }
        
        public string ScanId { get; set; }
        
        public Exception Exception { get; set; }
    }
}