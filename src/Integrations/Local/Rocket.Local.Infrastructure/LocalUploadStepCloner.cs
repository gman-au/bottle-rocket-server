using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<LocalUploadWorkflowStep, LocalUploadExecutionStep>(serviceProvider)
    {
        public override LocalUploadExecutionStep Clone(LocalUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.UploadPath = value.UploadPath;

            return result;
        }
    }
}