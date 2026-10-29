using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Auth;
using Rocket.Api.Contracts.TokenSessions;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Auth;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        IAuthenticator authenticator,
        IGlobalSettingsRepository globalSettingsRepository,
        ISessionManager sessionManager
    ) : ControllerBase
    {
        private const string RefreshCookie = "refresh_token";
        private static readonly TimeSpan RefreshCookieLifetime = TimeSpan.FromDays(30);

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private string CurrentSessionId => User.FindFirstValue(AuthConstants.SessionId)!;

        [AllowAnonymous]
        [HttpPost("login")]
        [EndpointSummary("Login")]
        [EndpointGroupName("Authentication")]
        [EndpointDescription("Send the user name / password to retrieve an access token and refresh token.")]
        [ProducesResponseType(
            typeof(AccountLoginResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] AccountLoginRequest request,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    authenticator
                        .AuthenticateAsync(
                            request.Username,
                            request.Password,
                            cancellationToken
                        );

            if (user == null)
                throw new RocketException(
                    "Invalid username or password",
                    ApiStatusCodeEnum.InvalidCredentials
                );

            var globalSettings =
                await
                    globalSettingsRepository
                        .GetGlobalSettingsAsync(cancellationToken);

            var response = await sessionManager.CreateSessionAsync(
                user.Id,
                user.IsAdmin,
                string.IsNullOrWhiteSpace(request.DeviceName) ? "Unknown device" : request.DeviceName,
                Request.Headers.UserAgent.ToString(),
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                cancellationToken
            );

            // I haven't seen User / GlobalSettings, so these are guesses. Fix the names to match.
            response.UserName = user.Username;
            response.Role = user.IsAdmin ? "Admin" : "User";
            response.DarkMode = user.DarkMode;
            response.CurrentLanguage = globalSettings.DefaultLanguage;

            return Respond(
                response,
                request.UseCookie
            );
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [EndpointSummary("Refresh")]
        [EndpointGroupName("Authentication")]
        [EndpointDescription(
            "Exchange a refresh token for a new access token. The refresh token is rotated on every call. " +
            "Web clients use the HttpOnly cookie, mobile clients send it in the body."
        )]
        [ProducesResponseType(
            typeof(AccountLoginResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshRequest request,
            CancellationToken cancellationToken
        )
        {
            var fromCookie =
                Request
                    .Cookies[RefreshCookie];

            var raw = fromCookie ?? request?.RefreshToken;

            if (string.IsNullOrEmpty(raw))
                return Unauthorized();

            var response =
                await
                    sessionManager
                        .RefreshAsync(
                            raw,
                            cancellationToken
                        );

            if (response == null)
            {
                Response
                    .Cookies
                    .Delete(
                        RefreshCookie,
                        CookieOptionsFor(null)
                    );

                return Unauthorized();
            }

            return Respond(
                response,
                useCookie: fromCookie != null
            );
        }

        [Authorize]
        [HttpPost("logout")]
        [EndpointSummary("Logout")]
        [EndpointGroupName("Authentication")]
        [EndpointDescription(
            "Logout / revoke the current session. If LogoutAll is set to 'true', it will revoke the sessions for all users."
        )]
        [ProducesResponseType(
            typeof(AccountLogoutResponse),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> Logout(
            [FromBody] AccountLogoutRequest request = null,
            CancellationToken cancellationToken = default
        )
        {
            var revokeAll = request?.LogoutAll ?? false;

            if (revokeAll)
            {
                await
                    sessionManager
                        .RevokeAllAsync(
                            UserId,
                            null,
                            cancellationToken
                        );
            }
            else
            {
                await
                    sessionManager
                        .RevokeAsync(
                            UserId,
                            CurrentSessionId,
                            cancellationToken
                        );
            }

            Response
                .Cookies
                .Delete(
                    RefreshCookie,
                    CookieOptionsFor(null)
                );

            return
                new AccountLogoutResponse
                    {
                        IsLoggedOut = true
                    }
                    .AsApiSuccess();
        }

        [Authorize]
        [HttpGet("sessions")]
        [EndpointSummary("List sessions")]
        [EndpointGroupName("Authentication")]
        [ProducesResponseType(
            typeof(MySessionsResponse),
            StatusCodes.Status200OK
        )]
        public async Task<IActionResult> Sessions(
            CancellationToken cancellationToken
        )
        {
            var sessions =
                await
                    sessionManager
                        .ListAsync(
                            UserId,
                            cancellationToken
                        );

            var sessionResults =
                sessions
                    .Select(s =>
                        new TokenSessionSpecifics
                        {
                            Id = s.Id,
                            DeviceName = s.DeviceName,
                            UserAgent = s.UserAgent,
                            IpAddress = s.IpAddress,
                            CreatedAt = s.CreatedAt,
                            LastLoginAt = s.LastLoginAt,
                            IsCurrent = s.Id == CurrentSessionId
                        }
                    )
                    .ToArray();

            return
                new MySessionsResponse
                    {
                        Sessions = sessionResults
                    }
                    .AsApiSuccess();
        }

        [Authorize]
        [HttpDelete("sessions/{id}")]
        [EndpointSummary("Revoke a session")]
        [EndpointGroupName("Authentication")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RevokeSession(
            string id,
            CancellationToken cancellationToken
        ) =>
            await
                sessionManager
                    .RevokeAsync(
                        UserId,
                        id,
                        cancellationToken
                    )
                ? NoContent()
                : NotFound();

        private IActionResult Respond(
            AccountLoginResponse response,
            bool useCookie
        )
        {
            if (useCookie)
            {
                Response
                    .Cookies
                    .Append(
                        RefreshCookie,
                        response.RefreshToken,
                        CookieOptionsFor(DateTimeOffset.UtcNow + RefreshCookieLifetime)
                    );

                response.RefreshToken = null; // JavaScript never sees it
            }

            return
                response
                    .AsApiSuccess();
        }

        private static CookieOptions CookieOptionsFor(
            DateTimeOffset? expires
        ) => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict, // use None if the web app is on a different site than the API
            Path = "/api/auth", // cookie paths are case-sensitive, so keep client URLs lowercase
            Expires = expires
        };
    }
}