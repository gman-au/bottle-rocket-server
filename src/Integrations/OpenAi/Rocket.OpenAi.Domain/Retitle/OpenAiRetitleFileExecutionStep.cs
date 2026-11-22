using Rocket.Domain.Executions;

namespace Rocket.OpenAi.Domain.Retitle
{
    public record OpenAiRetitleFileExecutionStep : BaseExecutionStep
    {
        public string ModelName { get; set; }

        public bool SummarizeAllFiles { get; set; }
    }
}