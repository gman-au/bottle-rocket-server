using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Infrastructure.Auth.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Auth.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddJwtAuthenticationServices(
            this IServiceCollection services,
            IWebHostEnvironment environment
        )
        {
            services
                .AddTransient<IRocketExceptionWrapper, RocketExceptionWrapper>()
                .AddTransient<IAuthenticator, ClientAuthenticator>();

            services
                .AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddSingleton<ISessionManager, SessionManager>()
                .AddSingleton<IAuthSecretProvider, AuthSecretProvider>()
                .AddSingleton<ITokenExpiryProvider, TokenExpiryProvider>();

            var provider =
                services
                    .BuildServiceProvider();

            var authSecretProvider =
                provider
                    .GetRequiredService<IAuthSecretProvider>();

            var jwtSecret =
                authSecretProvider
                    .Get();

            var key =
                Encoding
                    .ASCII
                    .GetBytes(jwtSecret);

            services
                .AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = !environment.IsDevelopment();
                        options.SaveToken = true;
                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(key),
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                ClockSkew = TimeSpan.Zero
                            };

                        options.Events =
                            new JwtBearerEvents
                            {
                                OnTokenValidated = async context =>
                                {
                                    var cancellationToken =
                                        context
                                            .HttpContext
                                            .RequestAborted;

                                    var sessionId =
                                        context
                                            .Principal?
                                            .FindFirstValue(AuthConstants.SessionId);

                                    var sessions =
                                        context
                                            .HttpContext
                                            .RequestServices
                                            .GetRequiredService<ISessionManager>();

                                    var logger = context.HttpContext.RequestServices
                                        .GetRequiredService<ILogger<JwtBearerEvents>>();

                                    logger
                                        .LogDebug(
                                            "Claims: {claims}",
                                            string
                                                .Join(
                                                    ", ",
                                                    context
                                                        .Principal!
                                                        .Claims
                                                        .Select(c => $"{c.Type}={c.Value}")
                                                )
                                        );

                                    if (sessionId is null || !await sessions.IsActiveAsync(
                                            sessionId,
                                            cancellationToken
                                        ))
                                    {
                                        var exceptionWrapper =
                                            context
                                                .HttpContext
                                                .RequestServices
                                                .GetRequiredService<IRocketExceptionWrapper>();

                                        context.Response.StatusCode =
                                            StatusCodes
                                                .Status401Unauthorized;

                                        await
                                            context
                                                .Response
                                                .WriteAsJsonAsync(
                                                    exceptionWrapper
                                                        .For(
                                                            new RocketException(
                                                                "Session revoked or expired",
                                                                ApiStatusCodeEnum.TokenHasExpired,
                                                                (int)HttpStatusCode.Unauthorized
                                                            )
                                                        )
                                                        .Value,
                                                    cancellationToken: cancellationToken
                                                );

                                        await
                                            context
                                                .Response
                                                .CompleteAsync();
                                    }
                                },
                                OnAuthenticationFailed = async context =>
                                {
                                    if (context.Exception is SecurityTokenExpiredException)
                                    {
                                        var exceptionWrapper =
                                            context
                                                .HttpContext
                                                .RequestServices
                                                .GetRequiredService<IRocketExceptionWrapper>();

                                        context.Response.StatusCode =
                                            StatusCodes
                                                .Status401Unauthorized;

                                        await
                                            context
                                                .Response
                                                .WriteAsJsonAsync(
                                                    exceptionWrapper
                                                        .For(
                                                            new RocketException(
                                                                "Session has expired - please login again",
                                                                ApiStatusCodeEnum.TokenHasExpired,
                                                                (int)HttpStatusCode.Unauthorized
                                                            )
                                                        )
                                                        .Value
                                                );

                                        context.Exception = null;

                                        await
                                            context
                                                .Response
                                                .CompleteAsync();
                                    }
                                }
                            };
                    }
                );

            return services;
        }
    }
}