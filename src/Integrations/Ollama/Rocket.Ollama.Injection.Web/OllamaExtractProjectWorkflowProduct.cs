using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Injection.Web
{
    public class OllamaExtractProjectWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract project tasks from image (Ollama)";

        public string Description => "Extract project task information from an image using an Ollama model.";

        public string[] Categories => [SkuConstants.TextRecognition, SkuConstants.ProjectManagement];

        public string HrefBase => "/MyWorkflow/Ollama/ExtractProject";

        public string ImagePath => "/img/ollama-logo.png";

        public bool DataLeavesYourServer => false;

        public string StepCode => OllamaDomainConstants.OllamaExtractProjectTasksWorkflowCode;
        
        public int[] InputTypes => OllamaDomainConstants.OllamaExtractProjectTaskInputTypes;

        public int OutputType => OllamaDomainConstants.OllamaExtractProjectTaskOutputType;
    }
}