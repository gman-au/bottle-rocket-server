using System.Linq;
using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions.Projects;
using Rocket.Page.Schemas.ProjectTaskTracker;

namespace Rocket.Infrastructure.Mapping.Transcriptions
{
    public class ProjectTranscriptionMapper
        : TranscriptionModelMapperBase<ProjectTranscription, ProjectTranscriptionSummary>
    {
        public override ProjectTranscription For(
            ProjectTranscriptionSummary value
        )
        {
            var result =
                base
                    .For(value);

            result.Notes = value?.Project?.Notes;
            result.Items =
                (value?.Project?.Items ?? [])
                .Select(o =>
                    new ProjectItem
                    {
                        Project = o.Project,
                        Task = o.Task,
                        DueDate = o.DueDate,
                        EstimatedTime = o.EstimatedTime
                    }
                );

            return result;
        }

        public override ProjectTranscriptionSummary From(
            ProjectTranscription value
        )
        {
            var result =
                base
                    .From(value);

            result.Project =
                new ProjectTaskTrackerSchema
                {
                    Notes = value?.Notes,
                    Items =
                        (value?
                            .Items ?? [])
                        .Select(o =>
                            new ProjectTaskTrackerItem
                            {
                                Project = o.Project,
                                Task = o.Task,
                                DueDate = o.DueDate,
                                EstimatedTime = o.EstimatedTime
                            }
                        )
                };

            return result;
        }
    }
}