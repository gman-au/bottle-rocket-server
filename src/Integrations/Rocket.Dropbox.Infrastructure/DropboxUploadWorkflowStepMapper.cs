using System;
using Rocket.Domain.Vendors.Dropbox;
using Rocket.Dropbox.Contracts;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<DropboxUploadWorkflowStep, DropboxUploadWorkflowStepSpecifics>(serviceProvider)
    {
        public override DropboxUploadWorkflowStep For(DropboxUploadWorkflowStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Subfolder = value.Subfolder;

            return result;
        }

        public override DropboxUploadWorkflowStepSpecifics From(DropboxUploadWorkflowStep value)
        {
            var result =
                base
                    .From(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}