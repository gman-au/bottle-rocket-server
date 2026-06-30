using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Mailgun.Domain;

namespace Rocket.Mailgun.Injection.Web
{
    public class MailgunSendEmailWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Send an email using Mailgun";

        public string Description => "Use the Mailgun API to send an email with the file data as an attachment.";

        public string[] Categories => [SkuConstants.FileForwarding];

        public string HrefBase => "/MyWorkflow/Mailgun";

        public string ImagePath => "/img/mailgun-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => MailgunDomainConstants.MailgunSendEmailWorkflowCode;
        
        public int[] InputTypes { get; set; } = MailgunDomainConstants.MailgunSendEmailInputTypes;

        public int OutputType => MailgunDomainConstants.MailgunSendEmailOutputType;
    }
}