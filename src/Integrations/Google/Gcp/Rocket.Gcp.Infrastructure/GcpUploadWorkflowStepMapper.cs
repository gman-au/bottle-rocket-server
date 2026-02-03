using System;
using Rocket.Gcp.Contracts;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<GcpUploadWorkflowStep, GcpUploadWorkflowStepSpecifics>(serviceProvider);
}