using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IAuthenticationManager
    {
        Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken);

        Task LogoutAsync();

        Task<bool> IsAuthenticatedAsync();

        Task<string> GetAuthorizationHeaderAsync();
        
        Task<string> GetUsernameAsync();
        
        Task<string> GetRoleAsync();
        
        Task<string> GetCurrentLanguageAsync();

        Task<bool> IsRootAdminAsync();

        event Action OnAuthenticationStateChanged;

        Task SetCurrentLanguageAsync(string language);
    }
}