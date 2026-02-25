using Rocket.Domain.Executions;

namespace Rocket.Ollama.Domain.Text
{
    public record OllamaExtractTextExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}