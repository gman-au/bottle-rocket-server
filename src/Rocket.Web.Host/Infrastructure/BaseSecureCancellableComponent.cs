using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Rocket.Interfaces;

namespace Rocket.Web.Host.Infrastructure
{
    public class BaseSecureCancellableComponent : BaseCancellableComponent
    {
        [Inject]
        protected IAuthenticationManager AuthenticationManager { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ILogger<BaseSecureCancellableComponent> Logger { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            Logger.LogWarning("Checking auth...");
        
            var isAuthenticated = 
                await 
                    AuthenticationManager
                        .IsAuthenticatedAsync();
        
            Logger
                .LogWarning("Auth result: {isAuth}", isAuthenticated);
        
            if (!isAuthenticated)
            {
                var returnUrl = 
                    NavigationManager
                        .ToBaseRelativePath(NavigationManager.Uri);
                
                NavigationManager
                    .NavigateTo($"/account/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
                
                return;
            }

            await 
                base
                    .OnInitializedAsync();
        }

        private async Task CheckAuthAsync()
        {
            var isAuthenticated = 
                await 
                    AuthenticationManager
                        .IsAuthenticatedAsync();
        
            if (!isAuthenticated)
                await InvokeAsync(RedirectToLogin);
        }

        private void RedirectToLogin()
        {
            var returnUrl = 
                NavigationManager
                    .ToBaseRelativePath(NavigationManager.Uri);
            
            NavigationManager
                .NavigateTo($"/account/login?returnUrl={Uri.EscapeDataString(returnUrl)}");
        }
    }
}