using Rocket.Domain.Enum;

namespace Rocket.Google.Domain
{
    public static class GoogleDomainConstants
    {
        public const string ConnectorName = "Google Connector";
        public const string ConnectorCode = "GOOGLE_PERSONAL";
        
        public const string UploadFileWorkflowCode = "GOOGLE_DRIVE_UPLOAD";
        
        public static readonly int[] GoogleDriveUploadInputTypes  =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int GoogleDriveUploadOutputType = (int)WorkflowFormatTypeEnum.Void;
    }
}