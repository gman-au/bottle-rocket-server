using System;
using System.Threading.Tasks;
using Rocket.Integrations.Domain;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common
{
    public class JobControl : IJobControl
    {
        public Func<JobLogEntry, Task> AppendLogMessageCallback { get; set;}
        
        public Func<JobStatusEntry, Task> UpdateStepStatusCallback { get; set;}
    }
}