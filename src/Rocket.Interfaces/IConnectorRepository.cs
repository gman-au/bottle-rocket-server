using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Core;

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
        ) where T : CoreConnector;

        Task<T> GetConnectorByNameAsync<T>(
            string userId,
            string name,
            CancellationToken cancellationToken
        ) where T : CoreConnector;

        Task<(IEnumerable<CoreConnector> records, long totalRecordCount)> FetchConnectorsAsync(
            string userId,
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );

        Task<(IEnumerable<CoreConnector> records, long totalRecordCount)> FetchConnectorsByCodeAndUserAsync(
            string userId,
            int? startIndex,
            int? recordCount,
            string code,
            CancellationToken cancellationToken
        );

        Task<CoreConnector> InsertConnectorAsync(
            CoreConnector coreConnector,
            CancellationToken cancellationToken
        );

        Task UpdateConnectorFieldAsync<TConnector, TField>(
            string connectorId,
            string userId,
            Expression<Func<TConnector, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        ) where TConnector : CoreConnector;

        Task<bool> DeleteConnectorAsync(
            string userId,
            string id,
            CancellationToken cancellationToken
        );
    }
}