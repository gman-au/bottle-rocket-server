using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Replicate.Domain;

namespace Rocket.Replicate.Injection.Web
{
    public class DataLabToExtractProjectWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract project (datalab-to/marker)";

        public string Description => "Extract project data from image using Replicate (model: datalab-to/marker)";

        public string[] Categories => [SkuConstants.TextRecognition, SkuConstants.ProjectManagement];

        public string HrefBase => "/MyWorkflow/Replicate/DataLabTo/Project";

        public string ImagePath => "/img/replicate-logo.png";

        public bool DataLeavesYourServer => true;

        public string StepCode => ReplicateDomainConstants.DataLabToExtractProjectWorkflowCode;
        
        public int[] InputTypes => ReplicateDomainConstants.ReplicateExtractInputTypes;

        public int OutputType => ReplicateDomainConstants.ReplicateExtractProjectTaskOutputType;
    }
}