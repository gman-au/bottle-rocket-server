using System;
using Rocket.Google.Contracts;
using Rocket.Google.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Google.Infrastructure
{
    public class GoogleDriveUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<GoogleDriveUploadExecutionStep, GoogleDriveUploadExecutionStepSpecifics>(serviceProvider);
}