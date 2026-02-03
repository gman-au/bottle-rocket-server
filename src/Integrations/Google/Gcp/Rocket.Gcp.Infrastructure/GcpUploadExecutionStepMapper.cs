using System;
using Rocket.Gcp.Contracts;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpUploadExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<GcpUploadExecutionStep, GcpUploadExecutionStepSpecifics>(serviceProvider);
}