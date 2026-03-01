using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Injection.Web
{
    public class OllamaExtractTextWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract raw text from image (Ollama)";

        public string Description => "Extract raw text from an image using an Ollama model.";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/Ollama/ExtractText";

        public string ImagePath => "/img/ollama-logo.png";

        public bool DataLeavesYourServer => false;

        public string StepCode => OllamaDomainConstants.OllamaExtractTextWorkflowCode;
        
        public int[] InputTypes => OllamaDomainConstants.OllamaExtractTextInputTypes;

        public int OutputType => OllamaDomainConstants.OllamaExtractTextOutputType;
    }
}