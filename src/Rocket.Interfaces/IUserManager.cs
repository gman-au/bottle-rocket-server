using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IUserManager
    {
        Task<User> CreateUserAccountAsync(
            string userName,
            string password,
            bool isTheNewAdmin,
            CancellationToken cancellationToken
        );

        Task DeactivateAdminAccountAsync(CancellationToken cancellationToken);

        Task UpdateAccountIsAdminAsync(
            string userId,
            bool value,
            CancellationToken cancellationToken
        );

        Task<User> GetUserByUserIdAsync(
            string userId,
            CancellationToken cancellationToken
        );

        Task UpdateAccountAsync(
            string userId,
            string userName,
            
            bool? isActive,
            bool? isAdmin,
            string newPassword,
            CancellationToken cancellationToken
        );

        Task UpdateDarkModePreferenceAsync(
            string userId,
            bool requestDarkMode,
            CancellationToken cancellationToken
        );
    }
}