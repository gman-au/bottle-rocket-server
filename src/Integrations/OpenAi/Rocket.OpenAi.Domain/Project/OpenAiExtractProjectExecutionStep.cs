using Rocket.Domain.Executions;

namespace Rocket.OpenAi.Domain.Project
{
    public record OpenAiExtractProjectExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}