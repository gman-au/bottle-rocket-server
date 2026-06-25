using System.Collections.Generic;

namespace Rocket.Domain.Dashboard
{
    public class ExecutionsSummary
    {
        public int TotalExecutions { get; set; }
        
        public IEnumerable<ExecutionByStatusTotal> ExecutionsByStatus { get; set; }
        
        public IEnumerable<ExecutionByWorkflowTotal> ExecutionsByWorkflow { get; set; }
    }
}