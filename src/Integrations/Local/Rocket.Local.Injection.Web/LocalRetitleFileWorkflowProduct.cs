using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Local.Domain;

namespace Rocket.Local.Injection.Web
{
    public class LocalRetitleFileWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Auto-title file (local)";

        public string Description => "Re-title a document file based on its contents.";

        public string[] Categories => [SkuConstants.LocalNetworkOnly, SkuConstants.FileRetitling];

        public string HrefBase => "/MyWorkflow/LocalRetitle";

        public string ImagePath => "/img/bottle-rocket-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillNotLeaveYourServer;

        public string StepCode => LocalDomainConstants.LocalRetitleWorkflowCode;
        
        public int[] InputTypes => LocalDomainConstants.LocalRetitleInputTypes;

        public int OutputType => LocalDomainConstants.LocalRetitleFileOutputType;
    }
}