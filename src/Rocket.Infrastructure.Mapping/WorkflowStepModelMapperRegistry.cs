using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public class WorkflowStepModelMapperRegistry(IEnumerable<IWorkflowStepModelMapper> modelMappers) : IWorkflowStepModelMapperRegistry
    {
        public IWorkflowStepModelMapper GetMapperForView(Type type)
        {
            var matching =
                modelMappers
                    .FirstOrDefault(o => o.ViewType == type);

            if (matching == null)
                throw new RocketException(
                    $"The system is missing a mapping configuration for {type.Name}. " +
                    "This is a developer error and should be reported.",
                    ApiStatusCodeEnum.DeveloperError
                );

            return matching;
        }

        public IWorkflowStepModelMapper GetMapperForDomain(Type type)
        {
            var matching =
                modelMappers
                    .FirstOrDefault(o => o.DomainType == type);

            if (matching == null)
                throw new RocketException(
                    $"The system is missing a mapping configuration for {type.Name}. " +
                    "This is a developer error and should be reported.",
                    ApiStatusCodeEnum.DeveloperError
                );

            return matching;
        }
    }
}