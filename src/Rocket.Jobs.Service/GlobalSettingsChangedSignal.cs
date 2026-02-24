using System.Threading;
using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Jobs.Service
{
    public class GlobalSettingsChangedSignal : IGlobalSettingsChangedSignal
    {
        private volatile TaskCompletionSource _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task WaitAsync(CancellationToken cancellationToken = default)
        {
            return
                _tcs
                    .Task
                    .WaitAsync(cancellationToken);
        }

        public void NotifyChanged()
        {
            var old =
                Interlocked
                    .Exchange(
                        ref _tcs,
                        new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)
                    );

            old
                .TrySetResult();
        }
    }
}