using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public class StepModelClonerRegistry(IEnumerable<IStepModelCloner> modelCloners) : IStepModelClonerRegistry
    {
        public IStepModelCloner GetClonerForWorkflowStep(Type type)
        {
            var matching =
                modelCloners
                    .FirstOrDefault(o => o.WorkflowType == type);

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