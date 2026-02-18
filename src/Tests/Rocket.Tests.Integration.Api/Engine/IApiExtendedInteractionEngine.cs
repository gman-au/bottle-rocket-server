using System.Threading.Tasks;
using Achar.Interfaces.Testing;

namespace Rocket.Tests.Integration.Api.Engine
{
    public interface IApiExtendedInteractionEngine : IApiInteractionEngine
    {
        void SetImageBase64(string imageBase64);
        
        Task SendMultiPartRequestAsync(
            string method,
            string contentType,
            string fileName
        );
    }
}