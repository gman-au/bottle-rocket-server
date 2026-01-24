using System;
using System.Collections.Generic;
using Rocket.Api.Contracts.Workflows;
using Rocket.Dropbox.Contracts;
using Rocket.MaxOcr.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class WorkflowStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadWorkflowStepSpecifics), "dropbox_upload_workflow" },
            { typeof(MaxOcrExtractWorkflowStepSpecifics), "maxocr_extract_workflow" },
            { typeof(EmailFileAttachmentStepSpecifics), "email_file_attachment" }
        };

        public static string GetWorkflowStepTypeDiscriminator(this Type type)
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