using System;

namespace Rocket.Interfaces
{
    public interface IExecutionStepModelMapperRegistry
    {
        IExecutionStepModelMapper GetMapperForView(Type type);

        IExecutionStepModelMapper GetMapperForDomain(Type type);
    }
}