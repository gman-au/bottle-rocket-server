using System;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<GoogleDriveUploadWorkflowStep, GoogleDriveUploadWorkflowStepSpecifics>(serviceProvider)
    {
        public override GoogleDriveUploadWorkflowStep For(GoogleDriveUploadWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.ParentFolderId = value.ParentFolderId;

            return result;
        }

        public override GoogleDriveUploadWorkflowStepSpecifics From(GoogleDriveUploadWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.ParentFolderId = value.ParentFolderId;

            return result;
        }
    }
}