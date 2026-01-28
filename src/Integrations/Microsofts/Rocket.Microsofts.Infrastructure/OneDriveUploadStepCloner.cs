using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Microsofts.Domain;

namespace Rocket.Microsofts.Infrastructure
{
    public class OneDriveUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<OneDriveUploadWorkflowStep, OneDriveUploadExecutionStep>(serviceProvider)
    {
        public override OneDriveUploadExecutionStep Clone(OneDriveUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}