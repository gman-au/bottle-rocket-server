using System;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpUploadStepCloner(IServiceProvider serviceProvider)
        : StepModelClonerBase<GcpUploadWorkflowStep, GcpUploadExecutionStep>(serviceProvider)
    {
        public override GcpUploadExecutionStep Clone(GcpUploadWorkflowStep value)
        {
            var result =
                base
                    .Clone(value);

            result.ParentFolderId = value.ParentFolderId;

            return result;
        }
    }
}