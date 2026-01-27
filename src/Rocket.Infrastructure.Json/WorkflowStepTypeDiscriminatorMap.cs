using System;
using System.Collections.Generic;
using Rocket.Api.Contracts.Workflows;
using Rocket.Diagnostics.Contracts;
using Rocket.Dropbox.Contracts;
using Rocket.MaxOcr.Contracts;
using Rocket.Notion.Contracts;
using Rocket.Ollama.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class WorkflowStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadWorkflowStepSpecifics), "dropbox_upload_workflow" },
            { typeof(MaxOcrExtractWorkflowStepSpecifics), "maxocr_extract_workflow" },
            { typeof(OllamaExtractWorkflowStepSpecifics), "ollama_extract_workflow" },
            { typeof(NotionUploadWorkflowStepSpecifics), "notion_upload_workflow" },
            { typeof(HelloWorldTextWorkflowStepSpecifics), "hello_world_text_workflow" },
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