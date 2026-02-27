using System;
using System.Collections.Generic;
using Rocket.Api.Contracts.Workflows;
using Rocket.Diagnostics.Contracts;
using Rocket.Dropbox.Contracts;
using Rocket.Gcp.Contracts;
using Rocket.Google.Contracts;
using Rocket.Microsofts.Contracts;
using Rocket.Notion.Contracts;
using Rocket.Ollama.Contracts.Project;
using Rocket.Ollama.Contracts.Text;
using Rocket.QuestPdf.Contracts;
using Rocket.Replicate.Contracts.Models.DataLabTo;
using Rocket.Replicate.Contracts.Models.DeepSeekOcr;

namespace Rocket.Infrastructure.Json
{
    public static class WorkflowStepTypeDiscriminatorMap
    {
        public static readonly Dictionary<Type, string> TypeDiscriminatorMap = new()
        {
            { typeof(DropboxUploadWorkflowStepSpecifics), "dropbox_upload_workflow" },
            { typeof(OllamaExtractTextWorkflowStepSpecifics), "ollama_extract_text_workflow" },
            { typeof(OllamaExtractProjectWorkflowStepSpecifics), "ollama_extract_project_workflow" },
            { typeof(NotionUploadNoteWorkflowStepSpecifics), "notion_upload_note_workflow" },
            { typeof(NotionUploadProjectTaskWorkflowStepSpecifics), "notion_upload_project_task_workflow" },
            { typeof(OneDriveUploadWorkflowStepSpecifics), "one_drive_upload_workflow" },
            { typeof(HelloWorldTextWorkflowStepSpecifics), "hello_world_text_workflow" },
            { typeof(HelloWorldProjectWorkflowStepSpecifics), "hello_world_project_workflow" },
            { typeof(GcpExtractWorkflowStepSpecifics), "gcp_extract_workflow" },
            { typeof(EmailFileAttachmentStepSpecifics), "email_file_attachment" },
            { typeof(OneNoteUploadWorkflowStepSpecifics), "one_note_upload_workflow" },
            { typeof(GoogleDriveUploadWorkflowStepSpecifics), "google_drive_upload_workflow" },
            { typeof(ConvertToPdfWorkflowStepSpecifics), "convert_to_pdf_workflow" },
            { typeof(DataLabToExtractTextWorkflowStepSpecifics), "replicate_data_lab_to_extract_text_workflow" },
            { typeof(DataLabToExtractProjectWorkflowStepSpecifics), "replicate_data_lab_to_extract_project_workflow" },
            { typeof(DeepSeekOcrExtractTextWorkflowStepSpecifics), "replicate_deep_seek_ocr_extract_text_workflow" }
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