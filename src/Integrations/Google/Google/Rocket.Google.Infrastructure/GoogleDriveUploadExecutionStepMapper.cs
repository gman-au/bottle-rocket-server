using System;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<GoogleDriveUploadExecutionStep, GoogleDriveUploadExecutionStepSpecifics>(serviceProvider)
    {
        public override GoogleDriveUploadExecutionStep For(GoogleDriveUploadExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentFolderId = value.ParentFolderId;

            return result;
        }

        public override GoogleDriveUploadExecutionStepSpecifics From(GoogleDriveUploadExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentFolderId = value.ParentFolderId;

            return result;
        }
    }
}