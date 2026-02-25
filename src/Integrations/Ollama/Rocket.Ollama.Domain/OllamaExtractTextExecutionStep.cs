using Rocket.Domain.Executions;

namespace Rocket.Ollama.Domain
{
    public record OllamaExtractTextExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}