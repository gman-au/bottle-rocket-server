using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Domain.Enum;
using Rocket.Domain.Jobs;

namespace Rocket.Integrations.Common.Extensions
{
    public static class BytesEx
    {
        private const string TextFileExtension = ".txt";
        private const string PdfFileExtension = ".pdf";
        private const string JsonFileExtension = ".json";


        public static ExecutionStepArtifact AsCompletedRawTextArtifact(
            this string value,
            ExecutionStepArtifact artifact = null
        )
        {
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Elements =
                    [
                        new ArtifactDataElement
                        {
                            Artifact = Encoding.Default.GetBytes(value),
                            FileExtension = TextFileExtension,
                            FileName = artifact?.FileName
                        }
                    ],
                    FileName = artifact?.FileName,
                    UserId = artifact?.UserId
                };
        }


        public static ExecutionStepArtifact AsCompletedPdfArtifact(
            this IEnumerable<(string, byte[])> values,
            ExecutionStepArtifact artifact = null
        )
        {
            var elements =
                (values ?? []).Select(o =>
                    new ArtifactDataElement
                    {
                        ScanId = o.Item1,
                        Artifact = o.Item2,
                        FileExtension = PdfFileExtension,
                        FileName = artifact?.FileName
                    }
                )
                .ToArray()
                .EnumerateUniquely();
            
            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.File,
                    Elements = elements,
                    FileName = artifact?.FileName,
                    UserId = artifact?.UserId
                };
        }

        /*
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
                    FileName = artifact?.FileName,
                    ScanId = artifact?.ScanId,
                    UserId = artifact?.UserId
                };
        }

        */
    }
}