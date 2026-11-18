using Rocket.Domain.Executions;

namespace Rocket.Markdig.Domain
{
    public record ConvertToEpubExecutionStep : BaseExecutionStep
    {
        public bool MergeFiles { get; set; }
    }
}