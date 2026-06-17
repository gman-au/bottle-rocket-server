using System.Text;
using System.Text.Json;
using Rocket.Domain.Enum;
using Rocket.Domain.Jobs;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Integrations.Common.Extensions
{
    public static class BytesEx
    {
        private const string TextFileExtension = ".txt";
        private const string PdfFileExtension = ".pdf";
        private const string JsonFileExtension = ".json";

        public static ExecutionStepArtifact AsCompletedRawTextArtifact(this string value, ExecutionStepArtifact artifact = null)
        {
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Artifact = Encoding.Default.GetBytes(value),
                    FileExtension = TextFileExtension,
                    FileName = artifact?.FileName
                };
        }

        public static ExecutionStepArtifact AsCompletedPdfArtifact(this byte[] value, ExecutionStepArtifact artifact = null)
        {
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.File,
                    Artifact = value,
                    FileExtension = PdfFileExtension,
                    FileName = artifact?.FileName
                };
        }

        public static ExecutionStepArtifact AsCompletedProjectTaskTrackerDataArtifact(this ProjectTaskTrackerSchema schema, ExecutionStepArtifact artifact = null)
        {
            var json =
                JsonSerializer
                    .Serialize(schema);
            
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData,
                    Artifact = Encoding.Default.GetBytes(json),
                    FileExtension = JsonFileExtension,
                    FileName = artifact?.FileName
                };
        }
    }
}