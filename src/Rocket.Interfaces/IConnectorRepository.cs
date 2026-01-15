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
        Task<bool> ConnectorExistsForUserAsync(
            string userId,
            string connectorName,
            CancellationToken cancellationToken
        );

        Task<T> GetConnectorByIdAsync<T>(
            string userId,
            string id,
            CancellationToken cancellationToken
        ) where T : BaseConnector;

        Task<T> GetConnectorByNameAsync<T>(
            string userId,
            string name,
            CancellationToken cancellationToken
        ) where T : BaseConnector;

        Task<(IEnumerable<BaseConnector> records, long totalRecordCount)> FetchConnectorsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );

        Task<BaseConnector> InsertConnectorAsync(
            BaseConnector baseConnector,
            CancellationToken cancellationToken
        );

        Task UpdateConnectorFieldAsync<TConnector, TField>(
            string connectorId,
            string userId,
            Expression<Func<TConnector, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) where TConnector : BaseConnector;

        Task<bool> DeleteConnectorAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );
    }
}