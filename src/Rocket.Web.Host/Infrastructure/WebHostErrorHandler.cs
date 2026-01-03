using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rocket.Domain.Exceptions;

namespace Rocket.Web.Host.Infrastructure
{
    public class WebHostErrorHandler(
        ISnackbar snackbar,
        NavigationManager navigationManager
        ) : IWebHostErrorHandler
    {
        public void HandleException(Exception ex)
        {
            switch (ex)
            {
                case HttpRequestException hex:
                    if (hex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        navigationManager
                            .NavigateTo("/login");
                    }
                    break;
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