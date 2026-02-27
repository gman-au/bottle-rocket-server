using Rocket.Domain.Executions;

namespace Rocket.Notion.Domain
{
    public record NotionUploadNoteExecutionStep : BaseExecutionStep
    {
        public string ParentNoteId { get; set; }
    }
}