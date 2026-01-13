using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IIntegrationHook
    {
        Task ProcessAsync(byte[] fileData);
    }
}