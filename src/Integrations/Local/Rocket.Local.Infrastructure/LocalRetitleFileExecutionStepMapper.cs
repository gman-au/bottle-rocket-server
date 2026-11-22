using System;
using Rocket.Infrastructure.Mapping;
using Rocket.Local.Contracts;
using Rocket.Local.Domain;

namespace Rocket.Local.Infrastructure
{
    public class LocalRetitleFileExecutionStepMapper(
        IServiceProvider serviceProvider
    )
        : ExecutionStepModelMapperBase<LocalRetitleFileExecutionStep, LocalRetitleFileExecutionStepSpecifics>(serviceProvider);
}