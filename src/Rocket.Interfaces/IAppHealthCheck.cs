using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IAppHealthCheck
    {
        Task PreRunAsync();
    }
}