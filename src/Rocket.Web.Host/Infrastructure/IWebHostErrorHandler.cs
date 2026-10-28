using System;
using System.Threading.Tasks;

namespace Rocket.Web.Host.Infrastructure
{
    public interface IWebHostErrorHandler
    {
        Task HandleExceptionAsync(Exception ex);
    }
}