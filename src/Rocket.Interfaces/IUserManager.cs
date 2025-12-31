using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IUserManager
    {
        Task<User> CreateUserAccountAsync(string username, string password, CancellationToken cancellationToken);

        Task DeactivateAdminAccountAsync(CancellationToken cancellationToken);
    }
}