using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Captures;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Utils;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        IAuthenticator authenticator,
        IAuthSecretProvider authSecretProvider,
        IGlobalSettingsRepository globalSettingsRepository,
        IConfiguration configuration
    ) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        [EndpointSummary("Login")]
        [EndpointGroupName("Authentication")]
        [EndpointDescription(
            """
            Login to the server. Send the user name / password to retrieve a bearer token for subsequent API calls.
            """
        )]
        [ProducesResponseType(
            typeof(ProcessCaptureResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] AccountLoginRequest model,
            CancellationToken cancellationToken
        )
        {
            var jwtExpirySeconds = 60 * 24 * 7;

            try
            {
                var jwtExpiry = configuration["JWT_EXPIRY_SECONDS"];
                jwtExpirySeconds = Convert.ToInt32(jwtExpiry);
            }
            catch (FormatException)
            {
            }

            var user =
                await
                    authenticator
                        .AuthenticateAsync(
                            model.Username,
                            model.Password,
                            cancellationToken
                        );

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var tokenHandler =
                new JwtSecurityTokenHandler();

            var key =
                Encoding
                    .ASCII
                    .GetBytes(
                        authSecretProvider
                            .Get()
                    );

            var globalSettings =
                await
                    globalSettingsRepository
                        .GetGlobalSettingsAsync(cancellationToken);

            var tokenDescriptor =
                new SecurityTokenDescriptor
                {
                    Subject =
                        new ClaimsIdentity(
                            [
                                new Claim(
                                    ClaimTypes.NameIdentifier,
                                    user.Id
                                ),
                                new Claim(
                                    ClaimTypes.Name,
                                    user.Username
                                ),
                                new Claim(
                                    ClaimTypes.Role,
                                    user.IsAdmin ? DomainConstants.AdminRole : DomainConstants.UserRole
                                )
                            ]
                        ),
                    Expires =
                        DateTime
                            .UtcNow
                            .AddSeconds(jwtExpirySeconds),
                    SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature
                        )
                };

            var token =
                tokenHandler
                    .CreateToken(tokenDescriptor);

            var tokenString =
                tokenHandler
                    .WriteToken(token);

            return
                new AccountLoginResponse
                    {
                        UserName = user.Username,
                        Role = user.IsAdmin ? DomainConstants.AdminRole : DomainConstants.UserRole,
                        DarkMode = user.DarkMode,
                        CurrentLanguage = globalSettings?.DefaultLanguage,
                        Token = tokenString
                    }
                    .AsApiSuccess();
        }
    }
}