using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Markdig.Domain;

namespace Rocket.Markdig.Injection.Web
{
    public class ConvertToEpubWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Convert data to EPUB file";

        public string Description => "Convert file data (markdown, text) to a EPUB file.";

        public string[] Categories => [SkuConstants.FileConversion, SkuConstants.LocalNetworkOnly];

        public string HrefBase => "/MyWorkflow/ConvertToEpub";

        public string ImagePath => "/img/epub-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillNotLeaveYourServer;

        public string StepCode => MarkdigDomainConstants.ConvertToEpubWorkflowCode;
        
        public int[] InputTypes => MarkdigDomainConstants.ConvertToEpubInputTypes;

        public int OutputType => MarkdigDomainConstants.ConvertToEpubOutputType;
    }
}