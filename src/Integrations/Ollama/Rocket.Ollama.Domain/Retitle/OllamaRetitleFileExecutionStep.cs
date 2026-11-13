using Rocket.Domain.Executions;

namespace Rocket.Ollama.Domain.Retitle
{
    public record OllamaRetitleFileExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}