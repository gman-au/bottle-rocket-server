using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldTextWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<HelloWorldTextWorkflowStep, HelloWorldTextWorkflowStepSpecifics>(serviceProvider)
    {
        public override HelloWorldTextWorkflowStep For(HelloWorldTextWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override HelloWorldTextWorkflowStepSpecifics From(HelloWorldTextWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}