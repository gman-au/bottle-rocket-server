using Rocket.Domain.Enum;

namespace Rocket.Notion.Domain
{
    public static class NotionDomainConstants
    {
        public const string ConnectorName = "Notion Integration Connector";
        public const string ConnectorCode = "NOTION_INTEGRATION";

        public const string NotionUploadNoteWorkflowCode = "NOTION_UPLOAD_NOTE";
        public const string NotionUploadProjectTaskWorkflowCode = "NOTION_UPLOAD_PROJECT_TASK";

        public static readonly int[] NotionUploadNoteInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int NotionUploadNoteOutputType = (int)WorkflowFormatTypeEnum.Void;

        public static readonly int[] NotionUploadProjectTaskInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData
        ];

        public const int NotionUploadProjectTaskOutputType = (int)WorkflowFormatTypeEnum.Void;
    }
}