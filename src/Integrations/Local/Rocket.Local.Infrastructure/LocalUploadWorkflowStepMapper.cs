using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Local.Contracts;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<LocalUploadWorkflowStep, LocalUploadWorkflowStepSpecifics>(serviceProvider)
    {
        public override LocalUploadWorkflowStep For(LocalUploadWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.UploadPath = value.UploadPath;

            return result;
        }

        public override LocalUploadWorkflowStepSpecifics From(LocalUploadWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.UploadPath = value.UploadPath;

            return result;
        }
    }
}