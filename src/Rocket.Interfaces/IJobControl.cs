using System;
using System.Threading.Tasks;
using Rocket.Integrations.Domain;

namespace Rocket.Interfaces
{
    public interface IJobControl
    {
        public Func<JobLogEntry, Task> AppendLogMessageCallback { get; set;}
        
        public Func<JobStatusEntry, Task> UpdateStepStatusCallback { get; set;}
    }
}