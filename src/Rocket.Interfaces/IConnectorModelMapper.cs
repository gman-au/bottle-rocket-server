using System;
using System.Threading.Tasks;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Domain.Connectors;

namespace Rocket.Interfaces
{
    public interface IConnectorModelMapper
    {
        Type DomainType { get; }
        Type ViewType { get; }
        BaseConnector For(object value);
        ConnectorSummary From(object value);

        Task PreUpdateAsync(object value);
        
        Task<ApiResponse> PostUpdateAsync(object value);
    }
    
    public interface IConnectorModelMapper<TDomain, TView> 
        : IConnectorModelMapper
        where TDomain : BaseConnector, new()
        where TView : ConnectorSummary, new()
    {
        bool AppliesFor(Type type);
        
        bool AppliesFrom(Type type);
        
        TDomain For(TView value);
        
        TView From(TDomain value);

        Task PreUpdateAsync(TView value);
        
        Task<ApiResponse> PostUpdateAsync(TDomain value);
    }
}