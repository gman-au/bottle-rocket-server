using System;
using System.Collections.Generic;
using Rocket.Diagnostics.Contracts;
using Rocket.Dropbox.Contracts;
using Rocket.Gcp.Contracts;
using Rocket.Google.Contracts;
using Rocket.Microsofts.Contracts;
using Rocket.Notion.Contracts;
using Rocket.Ollama.Contracts;
using Rocket.QuestPdf.Contracts;
using Rocket.Replicate.Contracts.Models.DataLabTo;
using Rocket.Replicate.Contracts.Models.DeepSeekOcr;

namespace Rocket.Infrastructure.Json
{
    public static class ExecutionStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadExecutionStepSpecifics), "dropbox_upload_execution" },
            { typeof(OllamaExtractExecutionStepSpecifics), "ollama_extract_execution" },
            { typeof(GcpExtractExecutionStepSpecifics), "gcp_extract_execution" },
            { typeof(HelloWorldTextExecutionStepSpecifics), "hello_world_text_execution" },
            { typeof(NotionUploadExecutionStepSpecifics), "notion_upload_execution" },
            { typeof(OneDriveUploadExecutionStepSpecifics), "one_drive_upload_execution" },
            { typeof(GoogleDriveUploadExecutionStepSpecifics), "google_drive_upload_execution" },
            { typeof(OneNoteUploadExecutionStepSpecifics), "one_note_upload_execution" },
            { typeof(ConvertToPdfExecutionStepSpecifics), "convert_to_pdf_execution" },
            { typeof(DataLabToExtractTextExecutionStepSpecifics), "replicate_data_lab_to_extract_text_execution" },
            { typeof(DeepSeekOcrExtractTextExecutionStepSpecifics), "replicate_deep_seek_ocr_extract_text_execution" }
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