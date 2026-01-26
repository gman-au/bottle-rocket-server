using Rocket.Domain.Executions;

namespace Rocket.Ollama.Domain
{
    public record OllamaExtractExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}