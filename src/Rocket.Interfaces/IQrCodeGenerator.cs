using System.Threading;
using System.Threading.Tasks;
using Rocket.Api.Contracts.Users;

namespace Rocket.Interfaces
{
    public interface IQrCodeGenerator
    {
        Task<string> GenerateUserQuickAuthCodeAsync(
            UserQuickAuth value,
            CancellationToken cancellationToken
        );
    }
}