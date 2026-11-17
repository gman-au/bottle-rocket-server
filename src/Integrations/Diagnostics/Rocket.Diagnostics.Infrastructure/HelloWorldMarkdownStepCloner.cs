using System;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldMarkdownStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<HelloWorldMarkdownWorkflowStep, HelloWorldMarkdownExecutionStep>(serviceProvider)
    {
        public override HelloWorldMarkdownExecutionStep Clone(HelloWorldMarkdownWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            return result;
        }
    }
}