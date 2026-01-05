using System;
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
        
        Task<string> GetUserNameAsync();

        Task<bool> IsAdminAsync();
        
        event Action OnAuthenticationStateChanged;
    }
}