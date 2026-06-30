using Rocket.Domain.Enum;

namespace Rocket.Mailgun.Domain
{
    public static class MailgunDomainConstants
    {
        public const string ConnectorName = "Mailgun Integration Connector";
        public const string ConnectorCode = "MAILGUN_INTEGRATION";

        public const string MailgunSendEmailWorkflowCode = "MAILGUN_SEND_EMAIL_FILE";

        public const int MailgunSendEmailOutputType = (int)WorkflowFormatTypeEnum.Void;

        public static readonly int[] MailgunSendEmailInputTypes =
        [
            (int)WorkflowFormatTypeEnum.File,
            (int)WorkflowFormatTypeEnum.RawTextData,
            (int)WorkflowFormatTypeEnum.ImageData
        ];
    }
}