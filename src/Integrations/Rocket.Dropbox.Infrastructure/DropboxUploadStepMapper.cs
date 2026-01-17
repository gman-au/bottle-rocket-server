using System;
using Rocket.Domain.Workflows;
using Rocket.Dropbox.Contracts;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxUploadStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<DropboxUploadStep, DropboxUploadStepSpecifics>(serviceProvider)
    {
        public override DropboxUploadStep For(DropboxUploadStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Subfolder = value.Subfolder;

            return result;
        }

        public override DropboxUploadStepSpecifics From(DropboxUploadStep value)
        {
            var result =
                base
                    .From(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}