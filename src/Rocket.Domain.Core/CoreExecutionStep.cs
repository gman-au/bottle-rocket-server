using System;
using System.Collections.Generic;

namespace Rocket.Domain.Core
{
    public abstract record CoreExecutionStep
    {
        public string Id { get; set; }
        
        public string ConnectorId { get; set; }

        public int[] InputTypes { get; set; }
        
        public int OutputType { get; set; }
        
        public string StepName { get; set; }
        
        public DateTime? RunDate { get; set; }
        
        public int ExecutionStatus { get; set; }
        
        public IEnumerable<CoreExecutionStep> ChildSteps { get; set; }
    }
}