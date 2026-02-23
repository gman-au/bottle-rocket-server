using Rocket.Api.Contracts;
using Rocket.Interfaces;

namespace Rocket.Ollama.Injection.Web
{
    public class OllamaExtractWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Extract text from image (Ollama)";

        public string Description => "Extract text from an image using an Ollama model.";

        public string[] Categories => [SkuConstants.TextRecognition];

        public string HrefBase => "/MyWorkflow/Ollama";

        public string ImagePath => "/img/ollama-logo.png";

        public bool DataLeavesYourServer => false;
    }
}