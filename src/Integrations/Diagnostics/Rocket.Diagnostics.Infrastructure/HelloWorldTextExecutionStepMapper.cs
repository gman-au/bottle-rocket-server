using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldTextExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<HelloWorldTextExecutionStep, HelloWorldTextExecutionStepSpecifics>(serviceProvider)
    {
        public override HelloWorldTextExecutionStep For(HelloWorldTextExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override HelloWorldTextExecutionStepSpecifics From(HelloWorldTextExecutionStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}