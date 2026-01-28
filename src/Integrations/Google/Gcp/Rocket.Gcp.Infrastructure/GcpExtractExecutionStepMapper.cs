using System;
using Rocket.Gcp.Contracts;
using Rocket.Gcp.Domain;
using Rocket.Infrastructure.Mapping;

namespace Rocket.Gcp.Infrastructure
{
    public class GcpExtractExecutionStepMapper(IServiceProvider serviceProvider)
        : ExecutionStepModelMapperBase<GcpExtractExecutionStep, GcpExtractExecutionStepSpecifics>(serviceProvider);
}