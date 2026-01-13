using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Connectors;

namespace Rocket.Interfaces
{
    public interface IConnectorRepository
    {
        Task<BaseConnector> SaveConnectorAsync(
            BaseConnector baseConnector,
            CancellationToken cancellationToken
        );

        Task<(IEnumerable<BaseConnector> records, long totalRecordCount)> FetchConnectorsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );

        Task<BaseConnector> FetchConnectorAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task<bool> DeleteConnectorAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );

        Task UpdateConnectorFieldAsync<TConnector, TField>(
            string connectorId,
            string userId,
            Expression<Func<TConnector, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) where TConnector : BaseConnector;

        Task<bool> ConnectorExistsForUserAsync(
            string userId,
            string connectorName,
            CancellationToken cancellationToken
        );
    }
}