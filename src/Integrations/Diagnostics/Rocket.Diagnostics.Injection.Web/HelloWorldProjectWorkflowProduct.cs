using Rocket.Api.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Web
{
    public class HelloWorldProjectWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Hello world (project output)";

        public string Description => "Produces a basic project tracker output, useful for testing child workflow steps.";

        public string[] Categories => [SkuConstants.Diagnostics, SkuConstants.ProjectManagement];

        public string HrefBase => "/MyWorkflow/Diagnostic/Project";

        public string ImagePath => "/img/bottle-rocket-logo.png";

        public bool DataLeavesYourServer => false;

        public string StepCode => DiagnosticDomainConstants.HelloWorldProjectWorkflowCode;
    }
}