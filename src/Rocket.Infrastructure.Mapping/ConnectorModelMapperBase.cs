using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Connectors;
using Rocket.Domain.Connectors;
using Rocket.Domain.Core.Utils;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Mapping
{
    public abstract class ConnectorModelMapperBase<TDomain, TView>(IServiceProvider serviceProvider)
        : IConnectorModelMapper<TDomain, TView>
        where TDomain : BaseConnector, new()
        where TView : ConnectorSummary, new()
    {
        public bool AppliesFor(Type type) => type == typeof(TView);
        public bool AppliesFrom(Type type) => type == typeof(TDomain);
        
        public virtual async Task PreUpdateAsync(object value) => await PreUpdateAsync((TView)value);
        public virtual async Task<ApiResponse> PostUpdateAsync(object value) => await PostUpdateAsync((TDomain)value);

        private IConnectorModelMapperRegistry _mapperRegistry;

        private IConnectorModelMapperRegistry MapperRegistry =>
            _mapperRegistry ??=
                serviceProvider
                    .GetRequiredService<IConnectorModelMapperRegistry>();

        public virtual TDomain For(TView value)
        {
            return new TDomain
            {
                Id = value.Id,
                UserId = value.UserId,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow
            };
        }

        public virtual TView From(TDomain value)
        {
            return new TView
            {
                Id = value.Id,
                UserId = value.UserId,
                ConnectorType =
                    DomainConstants
                        .ConnectorTypes
                        .GetValueOrDefault(
                            value.ConnectorType,
                            DomainConstants.UnknownType
                        ),
                ConnectorName = value.ConnectorName,
                CreatedAt = value.CreatedAt.ToLocalTime(),
                LastUpdatedAt = value.LastUpdatedAt?.ToLocalTime(),
                Status = (int)value.DetermineStatus()
            };
        }

        public virtual Task PreUpdateAsync(TView value) => Task.CompletedTask;

        public virtual Task<ApiResponse> PostUpdateAsync(TDomain value) => Task.FromResult(new ApiResponse());

        public Type DomainType => typeof(TDomain);
        public Type ViewType => typeof(TView);

        public BaseConnector For(object value) => For((TView)value);
        public ConnectorSummary From(object value) => From((TDomain)value);
    }
}