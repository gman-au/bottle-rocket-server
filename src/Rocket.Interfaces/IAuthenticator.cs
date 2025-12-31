using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IAuthenticator
    {
        Task<User> AuthenticateAsync(string username, string password, CancellationToken cancellationToken);
    }
}