using System;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldTextStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<HelloWorldTextWorkflowStep, HelloWorldTextExecutionStep>(serviceProvider)
    {
        public override HelloWorldTextExecutionStep Clone(HelloWorldTextWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}