using System;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<GoogleDriveUploadWorkflowStep, GoogleDriveUploadExecutionStep>(serviceProvider)
    {
        public override GoogleDriveUploadExecutionStep Clone(GoogleDriveUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ParentFolderId = value.ParentFolderId;

            return result;
        }
    }
}