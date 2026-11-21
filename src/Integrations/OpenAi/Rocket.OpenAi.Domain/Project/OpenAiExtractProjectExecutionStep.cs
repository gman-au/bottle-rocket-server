using Rocket.Domain.Executions;

namespace Rocket.OpenAi.Domain.Project
{
    public record OpenAiExtractProjectExecutionStep : BaseExecutionStep
    {
        public string FirstPassModelName { get; set; }
        
        public string SecondPassModelName { get; set; }
    }
}