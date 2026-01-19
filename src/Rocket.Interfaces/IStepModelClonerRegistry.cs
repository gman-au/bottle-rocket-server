using System;

namespace Rocket.Interfaces
{
    public interface IStepModelClonerRegistry
    {
        IStepModelCloner GetClonerForWorkflowStep(Type type);
    }
}