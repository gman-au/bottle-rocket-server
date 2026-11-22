using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface IJobControlBuilder
    {
        IJobControl Build(
            string batchId,
            string userId,
            string executionId,
            BaseExecutionStep executionStep
        );
    }
}