using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IAuthenticationManager
    {
        public bool IsAuthenticated();

        Task<bool> LoginAsync(string username, string password);

        Task LogoutAsync();

        string GetAuthorizationHeader();
    }
}