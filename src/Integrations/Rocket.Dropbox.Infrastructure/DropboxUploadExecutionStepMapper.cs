using System;
using Rocket.Domain.Vendors.Dropbox;
using Rocket.Dropbox.Contracts;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Dropbox.Infrastructure
{
    public class DropboxUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<DropboxUploadExecutionStep, DropboxUploadExecutionStepSpecifics>(serviceProvider)
    {
        public override DropboxUploadExecutionStep For(DropboxUploadExecutionStepSpecifics value)
        {
            var result =
                base
                    .For(value);

            result.Subfolder = value.Subfolder;

            return result;
        }

        public override DropboxUploadExecutionStepSpecifics From(DropboxUploadExecutionStep value)
        {
            var result =
                base
                    .From(value);

            result.Subfolder = value.Subfolder;

            return result;
        }
    }
}