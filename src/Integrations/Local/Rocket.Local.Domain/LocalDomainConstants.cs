using Rocket.Domain.Enum;

namespace Rocket.Local.Domain
{
    public static class LocalDomainConstants
    {
        public const string LocalUploadWorkflowCode = "LOCAL_UPLOAD";

        public static readonly int[] LocalUploadInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];

        public const int LocalUploadOutputType = (int)WorkflowFormatTypeEnum.Void;
    }
}