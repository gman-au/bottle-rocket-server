using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Web.Host.Components.Dialog;

namespace Rocket.Web.Host.Infrastructure
{
    public class WebHostErrorHandler(
        ISnackbar snackbar,
        IDialogService dialogService,
        NavigationManager navigationManager
    ) : IWebHostErrorHandler
    {
        public async Task HandleExceptionAsync(
            Exception ex
        )
        {
            switch (ex)
            {
                case HttpRequestException:
                    return;
                case RocketException rex:
                    switch (rex.ApiStatusCode)
                    {
                        case (int)ApiStatusCodeEnum.WorkflowExecutionAlreadyRunning:
                            return;
                        case (int)ApiStatusCodeEnum.TokenHasExpired:
                            var dialog =
                                await
                                    dialogService
                                        .ShowAsync<ExpiredSessionDialog>(
                                            null
                                        );

                            var result =
                                await
                                    dialog
                                        .Result;

                            if (!(result?.Canceled ?? true))
                            {
                                
                                navigationManager
                                    .NavigateTo(
                                        "/Account/Login",
                                        replace: true
                                    );
                            }

                            return;
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