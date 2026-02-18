using System.Threading.Tasks;
using Achar.Interfaces.Testing;

namespace Rocket.Tests.Integration.Api.Engine
{
    public interface IApiExtendedInteractionEngine : IApiInteractionEngine
    {
        Task SendMultiPartRequestAsync(
            string method,
            string endpoint,
            byte[] bytes,
            string contentType,
            string fileName
        );
    }
}