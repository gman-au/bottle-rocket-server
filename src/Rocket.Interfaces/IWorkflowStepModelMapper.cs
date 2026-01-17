using System;
using Rocket.Api.Contracts.Workflows;
using Rocket.Domain.Workflows;

namespace Rocket.Interfaces
{
    public interface IWorkflowStepModelMapper
    {
        Type DomainType { get; }
        Type ViewType { get; }
        BaseWorkflowStep For(object value);
        WorkflowStepSummary From(object value);
    }
    
    public interface IWorkflowStepModelMapper<TDomain, TView> 
        : IWorkflowStepModelMapper
        where TDomain : BaseWorkflowStep, new()
        where TView : WorkflowStepSummary, new()
    {
        bool AppliesFor(Type type);
        
        bool AppliesFrom(Type type);
        
        TDomain For(TView value);
        
        TView From(TDomain value);
    }
}