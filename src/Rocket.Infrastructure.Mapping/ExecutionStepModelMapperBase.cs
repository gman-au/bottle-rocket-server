using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Contracts.Executions;
using Rocket.Domain.Executions;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public abstract class ExecutionStepModelMapperBase<TDomain, TView>(IServiceProvider serviceProvider)
        : IExecutionStepModelMapper<TDomain, TView>
        where TDomain : BaseExecutionStep, new()
        where TView : ExecutionStepSummary, new()
    {
        public bool AppliesFor(Type type) => type == typeof(TView);
        public bool AppliesFrom(Type type) => type == typeof(TDomain);

        private IExecutionStepModelMapperRegistry _mapperRegistry;

        private IExecutionStepModelMapperRegistry MapperRegistry =>
            _mapperRegistry ??=
                serviceProvider
                    .GetRequiredService<IExecutionStepModelMapperRegistry>();

        public virtual TDomain For(TView value)
        {
            return new TDomain
            {
                Id = value.Id,
                ConnectorId = value.ConnectorId,
                ChildSteps =
                    (value.ChildSteps ?? [])
                    .Select(
                        child =>
                        {
                            var mapper =
                                MapperRegistry
                                    .GetMapperForView(child.GetType());

                            return
                                mapper
                                    .For(child);
                        }
                    )
            };
        }

        public virtual TView From(TDomain value)
        {
            return new TView
            {
                Id = value.Id,
                ConnectorId = value.ConnectorId,
                StepName = value.StepName,
                ExecutionStatus = value.ExecutionStatus,
                RunDate = value.RunDate,
                InputTypeName =
                    string.Join(
                        ", ",
                        value.InputTypes.Select(
                            x =>
                                DomainConstants
                                    .WorkflowFormatTypes
                                    .GetValueOrDefault(
                                        x,
                                        DomainConstants.UnknownType
                                    )
                        )
                    ),
                OutputTypeName =
                    DomainConstants
                        .WorkflowFormatTypes
                        .GetValueOrDefault(
                            value.OutputType,
                            DomainConstants.UnknownType
                        ),
                LogMessages = 
                    value
                        .LogMessages,
                ChildSteps =
                    (value.ChildSteps ?? [])
                    .Select(
                        child =>
                        {
                            var mapper =
                                MapperRegistry
                                    .GetMapperForDomain(child.GetType());

                            return
                                mapper
                                    .From(child);
                        }
                    )
                    .ToList()
            };
        }

        public Type DomainType => typeof(TDomain);
        public Type ViewType => typeof(TView);

        public BaseExecutionStep For(object value) => For((TView)value);
        public ExecutionStepSummary From(object value) => From((TDomain)value);
    }
}