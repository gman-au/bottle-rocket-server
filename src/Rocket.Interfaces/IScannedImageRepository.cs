using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.ScannedImages;

namespace Rocket.Interfaces
{
    public interface IScannedImageRepository : IMigratableRepository
    {
        Task UpdateScannedImageFieldAsync<TField>(
            string userId,
            string id,
            Expression<Func<ScannedImage, TField>> setter,
            TField value,
            CancellationToken cancellationToken
        );
    }
}