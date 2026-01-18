using System;
using System.Collections.Generic;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class WorkflowStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadStepSpecifics), "dropbox_upload" },
            { typeof(EmailFileAttachmentStepSpecifics), "email_file_attachment" }
        };

        public static string GetDiscriminator(this Type type)
        {
            return
                TypeDiscriminatorMap
                    .GetValueOrDefault(
                        type,
                        "base"
                    );
        }
    }
}