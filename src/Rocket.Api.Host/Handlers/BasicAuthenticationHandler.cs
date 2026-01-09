using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Handlers
{
    public class BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IAuthenticator authenticator
    )
        : AuthenticationHandler<AuthenticationSchemeOptions>(
            options,
            logger,
            encoder
        )
    {
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return 
                    AuthenticateResult
                        .Fail("Missing Authorization Header");
            }

            try
            {
                var authHeader = 
                    AuthenticationHeaderValue
                        .Parse(Request.Headers.Authorization);
                
                if (authHeader.Scheme != DomainConstants.Basic)
                {
                    return 
                        AuthenticateResult
                            .Fail("Invalid Authorization Scheme");
                }

                if (authHeader.Parameter == null)
                    return
                        AuthenticateResult
                            .Fail("Missing Authorization Header");
                
                var credentialBytes = 
                    Convert
                        .FromBase64String(authHeader.Parameter);
                
                var credentials = 
                    Encoding
                        .UTF8
                        .GetString(credentialBytes)
                        .Split(':', 2);
                    
                var username = credentials[0];
                var password = credentials[1];

                var user =
                    await
                        authenticator
                            .AuthenticateAsync(
                                username,
                                password,
                                Context.RequestAborted
                            );

                if (user == null)
                    return AuthenticateResult.Fail("Invalid Username or Password");

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? DomainConstants.AdminRole : DomainConstants.UserRole)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return 
                    AuthenticateResult
                        .Success(ticket);

            }
            catch (Exception ex)
            {
                Logger
                    .LogError(ex, "Error during authentication");

                return 
                    AuthenticateResult
                        .Fail("Invalid Authorization Header");
            }
        }
    }
}