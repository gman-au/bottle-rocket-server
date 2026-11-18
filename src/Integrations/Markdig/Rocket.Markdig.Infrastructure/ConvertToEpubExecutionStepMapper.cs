using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Markdig.Contracts;
using Rocket.Markdig.Domain;

namespace Rocket.Markdig.Infrastructure
{
    public class ConvertToEpubExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<ConvertToEpubExecutionStep, ConvertToEpubExecutionStepSpecifics>(serviceProvider)
    {
        public override ConvertToEpubExecutionStep For(ConvertToEpubExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override ConvertToEpubExecutionStepSpecifics From(ConvertToEpubExecutionStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}