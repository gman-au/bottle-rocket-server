using Rocket.Domain.Executions;

namespace Rocket.Microsofts.Domain
{
    public record OneNoteUploadExecutionStep : BaseExecutionStep
    {
        public string SectionId { get; set; }
    }
}