using System;
using System.Net.Http;
using System.Threading.Tasks;
using MudBlazor;
using Rocket.Domain.Exceptions;

namespace Rocket.Web.Host.Infrastructure
{
    public class WebHostErrorHandler(ISnackbar snackbar) : IWebHostErrorHandler
    {
        public void HandleException(Exception ex)
        {
            switch (ex)
            {
                case HttpRequestException:
                    return;
                case RocketException rex:
                    throw rex;
                case TaskCanceledException:
                case OperationCanceledException:
                    return;
                default:
                    snackbar
                        .Add(
                            ex.Message,
                            Severity.Error
                        );
                    break;
            }
        }
    }
}