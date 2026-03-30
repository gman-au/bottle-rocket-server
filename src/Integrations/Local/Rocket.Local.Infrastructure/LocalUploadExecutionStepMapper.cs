using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Local.Contracts;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<LocalUploadExecutionStep, LocalUploadExecutionStepSpecifics>(serviceProvider)
    {
        public override LocalUploadExecutionStep For(LocalUploadExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.UploadPath = value.UploadPath;

            return result;
        }

        public override LocalUploadExecutionStepSpecifics From(LocalUploadExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.UploadPath = value.UploadPath;

            return result;
        }
    }
}