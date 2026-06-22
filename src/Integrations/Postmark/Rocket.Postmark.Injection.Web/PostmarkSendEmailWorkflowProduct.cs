using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Postmark.Domain;

namespace Rocket.Postmark.Injection.Web
{
    public class PostmarkSendEmailWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Send an email using Postmark";

        public string Description => "Use the Postmark API to send an email with the file data as an attachment.";

        public string[] Categories => [SkuConstants.FileForwarding];

        public string HrefBase => "/MyWorkflow/Postmark";

        public string ImagePath => "/img/postmark-logo.jpg";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillLeaveYourServer;

        public string StepCode => PostmarkDomainConstants.PostmarkSendEmailWorkflowCode;
        
        public int[] InputTypes { get; set; } = PostmarkDomainConstants.PostmarkSendEmailInputTypes;

        public int OutputType => PostmarkDomainConstants.PostmarkSendEmailOutputType;
    }
}