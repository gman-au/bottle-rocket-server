using Rocket.Domain.Enum;

namespace Rocket.Postmark.Domain
{
    public static class PostmarkDomainConstants
    {
        public const string ConnectorName = "Postmark Integration Connector";
        public const string ConnectorCode = "POSTMARK_INTEGRATION";

        public const string PostmarkSendEmailWorkflowCode = "POSTMARK_SEND_EMAIL_FILE";

        public const int PostmarkSendEmailOutputType = (int)WorkflowFormatTypeEnum.Void;

        public static readonly int[] PostmarkSendEmailInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];
    }
}