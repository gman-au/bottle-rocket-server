using System.Collections.Generic;
using System.Linq;
using Rocket.Api.Contracts.Workflows;
using Rocket.Domain.Utils;
using Rocket.Domain.Workflows;
using Rocket.Dropbox.Contracts;

namespace Rocket.Api.Host.Extensions
{
    public static class BaseWorkflowStepEx
    {
        public static WorkflowStepSummary MapWorkflowStepToSpecific(this BaseWorkflowStep value)
        {
            var result =
                value switch
                {
                    DropboxUploadStep dropboxUploadStep =>
                        new DropboxUploadStepSpecifics
                        {
                            Subfolder = dropboxUploadStep.Subfolder
                        },
                    _ => new WorkflowStepSummary()
                };

            result.Id = value.Id;
            result.ConnectionId = value.ConnectionId;
            result.StepName = value.StepName;

            result.InputType = value.InputType;
            result.InputTypeName =
                DomainConstants
                    .WorkflowFormatTypes
                    .GetValueOrDefault(
                        value.InputType,
                        "Unknown"
                    );

            result.OutputType = value.OutputType;
            result.OutputTypeName =
                DomainConstants
                    .WorkflowFormatTypes
                    .GetValueOrDefault(
                        value.OutputType,
                        "Unknown"
                    );

            result.ChildSteps =
                (value.ChildSteps ?? [])
                .Select(o => o.MapWorkflowStepToSpecific());

            return result;
        }
    }
}