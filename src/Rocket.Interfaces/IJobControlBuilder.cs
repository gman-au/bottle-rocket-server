using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface IJobControlBuilder
    {
        IJobControl Build(
            string userId,
            string executionId,
            BaseExecutionStep executionStep
        );
    }
}