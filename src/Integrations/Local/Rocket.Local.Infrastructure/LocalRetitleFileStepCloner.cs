using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalRetitleFileStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<LocalRetitleFileWorkflowStep, LocalRetitleFileExecutionStep>(serviceProvider);
}