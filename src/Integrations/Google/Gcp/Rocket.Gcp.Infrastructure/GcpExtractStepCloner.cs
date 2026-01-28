using System;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpExtractStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<GcpExtractWorkflowStep, GcpExtractExecutionStep>(serviceProvider);
}