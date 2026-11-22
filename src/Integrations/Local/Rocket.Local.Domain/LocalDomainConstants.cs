using Rocket.Domain.Enum;

namespace Rocket.Local.Domain
{
    public static class LocalDomainConstants
    {
        public const string LocalUploadWorkflowCode = "LOCAL_UPLOAD";
        public const string LocalRetitleWorkflowCode = "LOCAL_RETITLE";

        public static readonly int[] LocalUploadInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData,
            (int)WorkflowFormatTypeEnum.Markdown
        ];

        public static readonly int[] LocalRetitleInputTypes =
        [
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.Markdown
        ];

        public const int LocalUploadOutputType = (int)WorkflowFormatTypeEnum.Void;

        public const int LocalRetitleFileOutputType = (int)WorkflowFormatTypeEnum.Inherited;
    }
}