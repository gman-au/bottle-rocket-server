using System;
using System.Collections.Generic;
using Rocket.Diagnostics.Contracts;
using Rocket.Dropbox.Contracts;
using Rocket.Gcp.Contracts;
using Rocket.MaxOcr.Contracts;
using Rocket.Microsofts.Contracts;
using Rocket.Notion.Contracts;
using Rocket.Ollama.Contracts;
using Rocket.QuestPdf.Contracts;

namespace Rocket.Infrastructure.Json
{
    public static class ExecutionStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadExecutionStepSpecifics), "dropbox_upload_execution" },
            { typeof(MaxOcrExtractExecutionStepSpecifics), "maxocr_extract_execution" },
            { typeof(OllamaExtractExecutionStepSpecifics), "ollama_extract_execution" },
            { typeof(GcpExtractExecutionStepSpecifics), "gcp_extract_execution" },
            { typeof(HelloWorldTextExecutionStepSpecifics), "hello_world_text_execution" },
            { typeof(NotionUploadExecutionStepSpecifics), "notion_upload_execution" },
            { typeof(OneDriveUploadExecutionStepSpecifics), "one_drive_upload_execution" },
            { typeof(GcpUploadExecutionStepSpecifics), "gcp_drive_upload_execution" },
            { typeof(OneNoteUploadExecutionStepSpecifics), "one_note_upload_execution" },
            { typeof(ConvertToPdfExecutionStepSpecifics), "convert_to_pdf_execution" }
        };

        public static string GetExecutionStepTypeDiscriminator(this Type type)
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