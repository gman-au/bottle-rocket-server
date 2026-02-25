using Rocket.Domain.Executions;

namespace Rocket.Ollama.Domain.Project
{
    public record OllamaExtractProjectExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}