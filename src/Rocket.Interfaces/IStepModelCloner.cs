using System;
using Rocket.Domain;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IStepModelCloner
    {
        Type WorkflowType { get; }
        Type ExecutionType { get; }
        BaseExecutionStep Clone(object value);
    }

    public interface IStepModelCloner<TWorkflow, TExecution>
        : IStepModelCloner
        where TWorkflow : BaseWorkflowStep, new()
        where TExecution : BaseExecutionStep, new()
    {
        bool AppliesFor(Type type);

        bool AppliesFrom(Type type);

        TExecution Clone(TWorkflow value);
    }
}