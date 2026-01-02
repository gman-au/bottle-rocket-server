using System;
using System.Threading;
using Microsoft.AspNetCore.Components;

namespace Rocket.Web.Host.Infrastructure
{
    public class CancellableComponentBase : ComponentBase, IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource;

        protected CancellationToken BaseCancellationToken =>
            (_cancellationTokenSource ??= new CancellationTokenSource())
            .Token;

        protected void BaseCancel()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
        }

        public virtual void Dispose()
        {
            if (_cancellationTokenSource == null) return;
            
            _cancellationTokenSource
                .Cancel();
            
            _cancellationTokenSource
                .Dispose();
            
            _cancellationTokenSource = null;
        }
    }
}