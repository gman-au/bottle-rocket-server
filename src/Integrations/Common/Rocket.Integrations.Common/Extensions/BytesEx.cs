using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Rocket.Domain.Enum;
using Rocket.Domain.Jobs;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Integrations.Common.Extensions
{
    public static class BytesEx
    {
        public static ExecutionStepArtifact AsCompletedRawTextArtifact(
            this IEnumerable<(string, string)> values,
            ExecutionStepArtifact artifact = null
        )
        {
            var elements =
                (values ?? []).Select(o =>
                    new ArtifactDataElement
                    {
                        ScanId = o.Item1,
                        Artifact = Encoding.Default.GetBytes(o.Item2),
                        FileExtension = CommonFileConstants.TextFileExtension,
                        FileName = artifact?.FileName
                    }
                )
                .ToArray()
                .EnumerateUniquely();

            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.RawTextData,
                    Elements = elements,
                    FileName = artifact?.FileName,
                    UserId = artifact?.UserId
                };
        }

        public static ExecutionStepArtifact AsCompletedPdfArtifact(
            this IEnumerable<ArtifactDataElement> values,
            ExecutionStepArtifact artifact = null
        )
        {
            var elements =
                (values ?? []).Select(o =>
                    new ArtifactDataElement
                    {
                        ScanId = o.ScanId,
                        Artifact = o.Artifact,
                        FileExtension = CommonFileConstants.PdfFileExtension,
                        FileName = o.FileName
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

        public static ExecutionStepArtifact AsCompletedProjectTaskTrackerDataArtifact(
            this IEnumerable<(string, ProjectTaskTrackerSchema)> values,
            ExecutionStepArtifact artifact = null
        )
        {
            var elements =
                (values ?? []).Select(o =>
                    new ArtifactDataElement
                    {
                        ScanId = o.Item1,
                        Artifact = Encoding.Default.GetBytes(
                            JsonSerializer
                                .Serialize(o.Item2)
                        ),
                        FileExtension = CommonFileConstants.JsonFileExtension,
                        FileName = artifact?.FileName
                    }
                )
                .ToArray()
                .EnumerateUniquely();

            return
                new ExecutionStepArtifact
                {
                    Result = (int)ExecutionStatusEnum.Completed,
                    ArtifactDataFormat = (int)WorkflowFormatTypeEnum.ProjectTaskTrackerData,
                    Elements = elements,
                    FileName = artifact?.FileName,
                    UserId = artifact?.UserId
                };
        }
    }
}