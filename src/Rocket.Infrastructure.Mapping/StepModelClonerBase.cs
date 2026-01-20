using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public abstract class StepModelClonerBase<TWorkflowStep, TExecutionStep>(IServiceProvider serviceProvider)
        : IStepModelCloner<TWorkflowStep, TExecutionStep>
        where TWorkflowStep : BaseWorkflowStep, new()
        where TExecutionStep : BaseExecutionStep, new()
    {
        public bool AppliesFor(Type type) => type == typeof(TExecutionStep);
        public bool AppliesFrom(Type type) => type == typeof(TWorkflowStep);

        public virtual TExecutionStep Clone(TWorkflowStep value)
        {
            return new TExecutionStep
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ConnectorId = value.ConnectorId,
                InputType = value.InputType,
                OutputType = value.OutputType,
                StepName = value.StepName,
                RunDate = null,
                ExecutionStatus = (int)ExecutionStatusEnum.NotRun,
                ChildSteps =
                    (value.ChildSteps ?? [])
                    .Select(
                        child =>
                        {
                            var mapper =
                                ClonerRegistry
                                    .GetClonerForWorkflowStep(child.GetType());

                            return
                                mapper
                                    .Clone(child);
                        }
                    )
                    .ToList()
            };
        }

        private IStepModelClonerRegistry _clonerRegistry;

        private IStepModelClonerRegistry ClonerRegistry =>
            _clonerRegistry ??=
                serviceProvider
                    .GetRequiredService<IStepModelClonerRegistry>();

        public Type WorkflowType => typeof(TWorkflowStep);
        public Type ExecutionType => typeof(TExecutionStep);

        public BaseExecutionStep Clone(object value) => Clone((TWorkflowStep)value);
    }
}