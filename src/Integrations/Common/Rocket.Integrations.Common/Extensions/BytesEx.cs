using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Domain.Extensions;
using Rocket.Domain.Jobs;
using Rocket.Interfaces;
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

        public static string ArtifactAsText(
            this ArtifactDataElement element
        )
        {
            var textBytes =
                element
                    .Artifact;

            if (textBytes == null)
                throw new RocketException(
                    "Artifact data for element is empty or not initialized; please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            return
                Encoding
                    .Default
                    .GetString(textBytes);
        }

        public static ProjectTaskTrackerSchema ArtifactAsProjectData(
            this ArtifactDataElement element
        )
        {
            var projectBytes =
                element
                    .Artifact;

            if (projectBytes == null)
                throw new RocketException(
                    "Artifact data for element is empty or not initialized; please check configuration",
                    ApiStatusCodeEnum.DeveloperError
                );

            try
            {
                var schema =
                    JsonSerializer
                        .Deserialize<ProjectTaskTrackerSchema>(projectBytes);

                return schema;
            }
            catch (JsonException)
            {
                throw new RocketException(
                    $"There was a problem loading the project task tracker artifact",
                    ApiStatusCodeEnum.DeveloperError
                );
            }
        }

        public static ExecutionStepArtifact AsArtifactWithRenamedFiles(
            this ExecutionStepArtifact artifact,
            IEnumerable<(string, string)> values
        )
        {
            var elements =
                (artifact?.Elements ?? []).Select(o =>
                    {
                        var matchedEntry =
                            values
                                .FirstOrDefault(v => o.ScanId == v.Item1);

                        if (matchedEntry != default)
                        {
                            var newFileName = matchedEntry.Item2;

                            return
                                new ArtifactDataElement
                                {
                                    ScanId = o.ScanId,
                                    Artifact = o.Artifact,
                                    FileExtension = o.FileExtension,
                                    FileName = newFileName.ToSanitizedFileName()
                                };
                        }

                        return o;
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