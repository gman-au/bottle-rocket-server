using System;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadWorkflowStepMapper(IServiceProvider serviceProvider)
        : WorkflowStepModelMapperBase<GoogleDriveUploadWorkflowStep, GoogleDriveUploadWorkflowStepSpecifics>(serviceProvider);
}