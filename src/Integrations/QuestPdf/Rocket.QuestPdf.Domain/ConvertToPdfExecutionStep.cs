using Rocket.Domain.Executions;

namespace Rocket.QuestPdf.Domain
{
    public record ConvertToPdfExecutionStep : BaseExecutionStep
    {
        public bool MergeFiles { get; set; }
    }
}