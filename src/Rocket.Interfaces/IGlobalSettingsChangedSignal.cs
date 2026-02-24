using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IGlobalSettingsChangedSignal
    {
        Task WaitAsync(CancellationToken cancellationToken = default);
        
        void NotifyChanged();
    }
}