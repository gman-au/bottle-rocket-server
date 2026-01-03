using System;

namespace Rocket.Web.Host.Infrastructure
{
    public interface IWebHostErrorHandler
    {
        void HandleException(Exception ex);
    }
}