using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldProjectWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<HelloWorldProjectWorkflowStep, HelloWorldProjectWorkflowStepSpecifics>(serviceProvider)
    {
        public override HelloWorldProjectWorkflowStep For(HelloWorldProjectWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override HelloWorldProjectWorkflowStepSpecifics From(HelloWorldProjectWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}