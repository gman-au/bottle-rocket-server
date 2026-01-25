using System;
using Rocket.Dropbox.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<DropboxUploadWorkflowStep, DropboxUploadExecutionStep>(serviceProvider)
    {
        public override DropboxUploadExecutionStep Clone(DropboxUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}