using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);
        
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
        
        Task UpdateLastLoginAsync(string userId, CancellationToken cancellationToken);

        Task DeactivateAdminUserAsync(CancellationToken cancellationToken);
    }
}