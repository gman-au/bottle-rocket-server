using System;
using Rocket.Infrastructure.Mapping;
using Rocket.MaxOcr.Contracts;
using Rocket.MaxOcr.Domain;

namespace Rocket.MaxOcr.Infrastructure
{
    public class MaxOcrExtractExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<MaxOcrExtractExecutionStep, MaxOcrExtractExecutionStepSpecifics>(serviceProvider)
    {
        public override MaxOcrExtractExecutionStep For(MaxOcrExtractExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override MaxOcrExtractExecutionStepSpecifics From(MaxOcrExtractExecutionStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}