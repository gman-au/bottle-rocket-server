using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface ITranscriptionControlBuilder
    {
        ITranscriptionControl Build(
            string batchId,
            string userId,
            string executionId,
            BaseExecutionStep executionStep
        );
    }
}