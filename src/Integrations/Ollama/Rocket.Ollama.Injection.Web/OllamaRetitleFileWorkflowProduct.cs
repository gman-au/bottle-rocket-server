using Rocket.Api.Contracts;
using Rocket.Interfaces;
using Rocket.Ollama.Domain;

namespace Rocket.Ollama.Injection.Web
{
    public class OllamaRetitleFileWorkflowProduct : ISkuWorkflow
    {
        public string Name => "Auto-title file (Ollama)";

        public string Description => "Re-title a document file using an Ollama model.";

        public string[] Categories => [SkuConstants.FileRetitling];

        public string HrefBase => "/MyWorkflow/Ollama/RetitleFile";

        public string ImagePath => "/img/ollama-logo.png";

        public int DataLeavesYourServer => (int)SkuDataExposureEnum.DataMightLeaveYourServer;

        public string StepCode => OllamaDomainConstants.OllamaRetitleFileWorkflowCode;
        
        public int[] InputTypes => OllamaDomainConstants.OllamaRetitleFileInputTypes;

        public int OutputType => OllamaDomainConstants.OllamaRetitleFileOutputType;
    }
}