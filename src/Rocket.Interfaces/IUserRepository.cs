using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);

        Task<User> GetUserByUserIdAsync(string userId, CancellationToken cancellationToken);

        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);

        Task UpdateUserFieldAsync<T>(
            string userId,
            Expression<Func<User, T>> setter,
            T value,
            CancellationToken cancellationToken
        );

        Task DeactivateAdminUserAsync(CancellationToken cancellationToken);
        
        Task<(IEnumerable<User> records, long totalRecordCount)> FetchUsersAsync(
            int startIndex,
            int recordCount,
            CancellationToken cancellationToken
        );
    }
}