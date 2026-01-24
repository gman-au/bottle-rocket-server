using System;
using System.Net.Http;
using System.Threading.Tasks;
using MudBlazor;
using Rocket.Domain.Enum;
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
                    switch (rex.ApiStatusCode)
                    {
                        case (int)ApiStatusCodeEnum.WorkflowExecutionAlreadyRunning:
                            break;
                        default:
                            throw rex;
                    }
                    break;
                case TaskCanceledException:
                case OperationCanceledException:
                    return;
            }
            snackbar
                .Add(
                    ex.Message,
                    Severity.Error
                );
        }
    }
}