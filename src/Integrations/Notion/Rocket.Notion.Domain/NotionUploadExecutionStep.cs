using Rocket.Domain.Executions;

namespace Rocket.Notion.Domain
{
    public record NotionUploadExecutionStep : BaseExecutionStep
    {
        public string ParentNoteId { get; set; }
    }
}