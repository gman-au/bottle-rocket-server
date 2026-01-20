using Rocket.Api.Contracts.Executions;

namespace Rocket.Interfaces
{
    public interface IExecutionMermaidConverter
    {
        string Convert(ExecutionSummary execution);
    }
}