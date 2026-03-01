using System;
using Rocket.Diagnostics.Contracts;
using Rocket.Diagnostics.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Diagnostics.Infrastructure
{
    public class HelloWorldProjectExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<HelloWorldProjectExecutionStep, HelloWorldProjectExecutionStepSpecifics>(serviceProvider)
    {
        public override HelloWorldProjectExecutionStep For(HelloWorldProjectExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override HelloWorldProjectExecutionStepSpecifics From(HelloWorldProjectExecutionStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}