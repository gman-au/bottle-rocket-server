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

        public static ExecutionStepArtifact AsCompletedRawTextArtifact(this string value)
        {
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Artifact = Encoding.Default.GetBytes(value),
                    FileExtension = TextFileExtension
                };
        }

        public static ExecutionStepArtifact AsCompletedPdfArtifact(this byte[] value)
        {
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.File,
                    Artifact = value,
                    FileExtension = PdfFileExtension
                };
        }

        public static ExecutionStepArtifact AsCompletedProjectTaskTrackerDataArtifact(this ProjectTaskTrackerSchema schema)
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
                    FileExtension = JsonFileExtension
                };
        }
    }
}