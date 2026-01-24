using System;
using System.Linq;
using Rocket.Domain.Enum;
using Rocket.Domain.Executions;
using Rocket.Domain.Workflows;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public class WorkflowCloner(IStepModelClonerRegistry clonerRegistry) : IWorkflowCloner
    {
        public Execution Clone(
            Workflow workflow,
            string scanId, 
            string scanThumbnailBase64,
            string scanContentType
        )
        {
            return new Execution
            {
                UserId = workflow.UserId,
                ScanId = scanId,
                ThumbnailBase64 = scanThumbnailBase64,
                ContentType = scanContentType,
                WorkflowId = workflow.Id,
                MatchingPageSymbol = workflow.MatchingPageSymbol,
                CreatedAt = DateTime.UtcNow,
                RunDate = null,
                Name = workflow.Name,
                ExecutionStatus = (int)ExecutionStatusEnum.NotRun,
                Steps =
                    (workflow.Steps ?? [])
                    .Select(o =>
                        {
                            var mapper =
                                clonerRegistry
                                    .GetClonerForWorkflowStep(o.GetType());

                            return
                                mapper
                                    .Clone(o);
                        }
                    )
            };
        }
    }
}