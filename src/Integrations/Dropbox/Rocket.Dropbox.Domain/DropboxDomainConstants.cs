using Rocket.Domain.Enum;

namespace Rocket.Dropbox.Domain
{
    public static class DropboxDomainConstants
    {
        public const string ConnectorName = "Dropbox App Connector";
        public const string ConnectorCode = "DROPBOX_APP";
        
        public const string UploadWorkflowCode = "DROPBOX_UPLOAD";

        public static readonly int[] DropboxUploadInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData,
            (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData
        ];

        public const int DropboxUploadOutputType = (int)WorkflowFormatTypeEnum.Void;
    }
}