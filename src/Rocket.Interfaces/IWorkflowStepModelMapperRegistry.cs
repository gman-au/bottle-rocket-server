using System;

namespace Rocket.Interfaces
{
    public interface IWorkflowStepModelMapperRegistry
    {
        IWorkflowStepModelMapper GetMapperForView(Type type);

        IWorkflowStepModelMapper GetMapperForDomain(Type type);
    }
}