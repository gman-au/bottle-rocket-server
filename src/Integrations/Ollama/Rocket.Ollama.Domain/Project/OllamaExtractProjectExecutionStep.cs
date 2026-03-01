using Rocket.Domain.Executions;

namespace Rocket.Ollama.Domain.Project
{
    public record OllamaExtractProjectExecutionStep : BaseExecutionStep
    {
        public string FirstPassModelName { get; set; }
        
        public string SecondPassModelName { get; set; }
    }
}