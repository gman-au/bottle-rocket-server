using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldMarkdownExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<HelloWorldMarkdownExecutionStep, HelloWorldMarkdownExecutionStepSpecifics>(serviceProvider)
    {
        public override HelloWorldMarkdownExecutionStep For(HelloWorldMarkdownExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override HelloWorldMarkdownExecutionStepSpecifics From(HelloWorldMarkdownExecutionStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}