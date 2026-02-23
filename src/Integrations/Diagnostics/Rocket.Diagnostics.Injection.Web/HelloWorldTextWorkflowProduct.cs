using Rocket.Api.Contracts;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Web
{
    public class HelloWorldTextWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Hello world (text output)";

        public string Description => "Produces a simple text output, useful for testing child workflow steps.";

        public string[] Categories => [SkuConstants.Diagnostics];

        public string HrefBase => "/MyWorkflow/Diagnostic";

        public string ImagePath => "/img/bottle-rocket-logo.png";

        public bool DataLeavesYourServer => false;
    }
}