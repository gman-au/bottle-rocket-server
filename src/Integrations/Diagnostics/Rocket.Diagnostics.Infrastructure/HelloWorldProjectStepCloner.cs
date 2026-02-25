using System;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldProjectStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<HelloWorldProjectWorkflowStep, HelloWorldProjectExecutionStep>(serviceProvider)
    {
        public override HelloWorldProjectExecutionStep Clone(HelloWorldProjectWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}