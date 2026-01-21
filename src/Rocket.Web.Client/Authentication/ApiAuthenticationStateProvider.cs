using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Web.Client.Authentication
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILogger<ApiAuthenticationStateProvider> _logger;

        public ApiAuthenticationStateProvider(
            IAuthenticationManager authenticationManager,
            ILogger<ApiAuthenticationStateProvider> logger
        )
        {
            _authenticationManager = authenticationManager;
            _logger = logger;
            _authenticationManager.OnAuthenticationStateChanged += OnAuthStateChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var isAuthenticated = await _authenticationManager.IsAuthenticatedAsync();
            var username = await _authenticationManager.GetUsernameAsync();
            var role = await _authenticationManager.GetRoleAsync();
            
            _logger
                .LogInformation(
                    "GetAuthenticationStateAsync: IsAuthenticated = {isAuth}",
                    isAuthenticated
                );

            var identity =
                isAuthenticated
                    ? new ClaimsIdentity(
                        [
                            new Claim(ClaimTypes.Name, username ?? "User"),
                            new Claim(ClaimTypes.Role, role ?? "") 
                        ],
                        DomainConstants
                            .BasicAuthentication
                    )
                    : new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            return 
                new AuthenticationState(user);
        }

        private void OnAuthStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}