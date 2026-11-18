using Rocket.Api.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Interfaces;

namespace Rocket.Diagnostics.Injection.Web
{
    public class HelloWorldMarkdownWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Hello world (markdown output)";

        public string Description => "Produces a simple markdown output, useful for testing child workflow steps.";

        public string[] Categories => [SkuConstants.Diagnostics];

        public string HrefBase => "/MyWorkflow/Diagnostic/Markdown";

        public string ImagePath => "/img/bottle-rocket-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataWillNotLeaveYourServer;

        public string StepCode => DiagnosticDomainConstants.HelloWorldMarkdownWorkflowCode;
        
        public int[] InputTypes => DiagnosticDomainConstants.HelloWorldMarkdownInputTypes;
        
        public int OutputType => DiagnosticDomainConstants.HelloWorldMarkdownOutputType;
    }
}