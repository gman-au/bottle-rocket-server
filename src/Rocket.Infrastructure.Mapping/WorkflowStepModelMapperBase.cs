using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Contracts.Workflows;
using Rocket.Domain.Utils;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public abstract class WorkflowStepModelMapperBase<TDomain, TView>(IServiceProvider serviceProvider)
        : IWorkflowStepModelMapper<TDomain, TView>
        where TDomain : BaseWorkflowStep, new()
        where TView : WorkflowStepSummary, new()
    {
        public bool AppliesFor(Type type) => type == typeof(TView);
        public bool AppliesFrom(Type type) => type == typeof(TDomain);

        private IWorkflowStepModelMapperRegistry _mapperRegistry;

        private IWorkflowStepModelMapperRegistry MapperRegistry =>
            _mapperRegistry ??=
                serviceProvider
                    .GetRequiredService<IWorkflowStepModelMapperRegistry>();

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
                RequiresConnectorCode = value.RequiresConnectorCode,
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

        public BaseWorkflowStep For(object value) => For((TView)value);
        public WorkflowStepSummary From(object value) => From((TDomain)value);
    }
}