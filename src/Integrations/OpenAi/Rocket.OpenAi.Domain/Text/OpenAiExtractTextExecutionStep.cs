using Rocket.Domain.Executions;

namespace Rocket.OpenAi.Domain.Text
{
    public record OpenAiExtractTextExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }
    }
}