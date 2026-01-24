using System;
using Rocket.Domain.Core;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IStepModelCloner
    {
        Type WorkflowType { get; }
        Type ExecutionType { get; }
        CoreExecutionStep Clone(object value);
    }

    public interface IStepModelCloner<TWorkflow, TExecution>
        : IStepModelCloner
        where TWorkflow : CoreWorkflowStep, new()
        where TExecution : CoreExecutionStep, new()
    {
        bool AppliesFor(Type type);

        bool AppliesFrom(Type type);

        TExecution Clone(TWorkflow value);
    }
}