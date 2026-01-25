using System;
using Rocket.Api.Contracts.Executions;
using Rocket.Domain.Executions;

namespace Rocket.Interfaces
{
    public interface IExecutionStepModelMapper
    {
        Type DomainType { get; }
        Type ViewType { get; }
        BaseExecutionStep For(object value);
        ExecutionStepSummary From(object value);
    }
    
    public interface IExecutionStepModelMapper<TDomain, TView> 
        : IExecutionStepModelMapper
        where TDomain : BaseExecutionStep, new()
        where TView : ExecutionStepSummary, new()
    {
        bool AppliesFor(Type type);
        
        bool AppliesFrom(Type type);
        
        TDomain For(TView value);
        
        TView From(TDomain value);
    }
}