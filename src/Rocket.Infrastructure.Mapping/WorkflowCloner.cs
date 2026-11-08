using System;
using System.Linq;
using MongoDB.Bson;
using Rocket.Domain.Batches;
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
            Batch batch
        )
        {
            return new Execution
            {
                UserId = workflow.UserId,
                BatchId = batch.Id,
                WorkflowId = workflow.Id,
                CreatedAt = DateTime.UtcNow,
                RunDate = null,
                Archived = false,
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
                    ),
                    Elements = 
                        (batch.Scans ?? [])
                        .Select(o =>
                            new ExecutionElement
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                ScanId = o.Id,
                                ExecutionStatus = (int)ExecutionStatusEnum.NotRun,
                                LogMessages = [],
                                ThumbnailBase64 = o.ThumbnailBase64,
                                ContentType = o.ContentType,
                                Vendor = o.Vendor
                            }
                        )
            };
        }
    }
}