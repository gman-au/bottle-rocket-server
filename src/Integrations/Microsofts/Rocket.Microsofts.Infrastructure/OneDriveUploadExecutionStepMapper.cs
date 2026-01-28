using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<OneDriveUploadExecutionStep, OneDriveUploadExecutionStepSpecifics>(serviceProvider)
    {
        public override OneDriveUploadExecutionStep For(OneDriveUploadExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Subfolder = value.Subfolder;

            return result;
        }

        public override OneDriveUploadExecutionStepSpecifics From(OneDriveUploadExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}