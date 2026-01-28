using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsoft.Contracts;
using Rocket.Microsoft.Domain;

namespace Rocket.Microsoft.Infrastructure
{
    public class OneDriveUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<OneDriveUploadWorkflowStep, OneDriveUploadWorkflowStepSpecifics>(serviceProvider)
    {
        public override OneDriveUploadWorkflowStep For(OneDriveUploadWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Subfolder = value.Subfolder;

            return result;
        }

        public override OneDriveUploadWorkflowStepSpecifics From(OneDriveUploadWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}