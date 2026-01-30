using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Contracts;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
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