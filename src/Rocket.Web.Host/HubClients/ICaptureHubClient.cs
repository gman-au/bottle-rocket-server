using System;
using System.Threading.Tasks;

namespace Rocket.Web.Host.HubClients
{
    public interface ICaptureHubClient : IAsyncDisposable
    {
        event Func<Task> OnNewCaptureReceived;
        
        event Func<Task> OnNewExecutionUpdateReceived;
        
        event Func<bool, Task> OnNewConnectorUpdateReceived;
        
        Task StartAsync();
        
        Task StopAsync();
        
        bool IsConnected { get; }
    }
}