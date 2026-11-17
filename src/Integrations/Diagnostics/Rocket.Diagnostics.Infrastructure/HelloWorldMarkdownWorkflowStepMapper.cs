using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldMarkdownWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<HelloWorldMarkdownWorkflowStep, HelloWorldMarkdownWorkflowStepSpecifics>(serviceProvider)
    {
        public override HelloWorldMarkdownWorkflowStep For(HelloWorldMarkdownWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override HelloWorldMarkdownWorkflowStepSpecifics From(HelloWorldMarkdownWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}