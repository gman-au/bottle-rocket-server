using Rocket.Domain.Enum;

namespace Rocket.Microsofts.Domain
{
    public static class MicrosoftDomainConstants
    {
        public const string ConnectorName = "Microsoft App Connector";
        public const string ConnectorCode = "MICROSOFT_APP";

        public const string OneDriveUploadWorkflowCode = "MICROSOFT_ONEDRIVE_UPLOAD";
        public const string OneNoteUploadWorkflowCode = "MICROSOFT_ONENOTE_UPLOAD";

        public static readonly int[] OneNoteUploadInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int OneNoteUploadOutputType = (int)WorkflowFormatTypeEnum.Void;

        public static readonly int[] OneDriveUploadInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int OneDriveUploadOutputType = (int)WorkflowFormatTypeEnum.Void;
    }
}