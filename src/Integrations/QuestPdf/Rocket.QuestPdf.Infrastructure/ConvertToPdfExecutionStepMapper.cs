using System;
using Rocket.Infrastructure.Mapping;
using Rocket.QuestPdf.Contracts;
using Rocket.QuestPdf.Domain;

namespace Rocket.QuestPdf.Infrastructure
{
    public class ConvertToPdfExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<ConvertToPdfExecutionStep, ConvertToPdfExecutionStepSpecifics>(serviceProvider)
    {
        public override ConvertToPdfExecutionStep For(ConvertToPdfExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            return result;
        }

        public override ConvertToPdfExecutionStepSpecifics From(ConvertToPdfExecutionStep value)
        {
            var result =
                base
                    .From(value);

            return result;
        }
    }
}