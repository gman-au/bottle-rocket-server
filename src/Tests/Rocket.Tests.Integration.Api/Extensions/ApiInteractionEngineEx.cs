using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Achar.Infrastructure.Api.Extensions;
using Achar.Infrastructure.Testing.Extensions;
using Achar.Interfaces.Testing;
using Rocket.Tests.Integration.Api.Engine;

namespace Rocket.Tests.Integration.Api.Extensions
{
    internal static class ApiInteractionEngineEx
    {
        private const string AdminIdentifier = "Admin";
        private const string AdminUserName = "admin";
        private const string AdminPassword = "password123";

        public static readonly Dictionary<string, Tuple<string, string>> TestUsers = new()
        {
            {
                "Admin", new Tuple<string, string>(
                    AdminUserName,
                    AdminPassword
                )
            },
            {
                "John", new Tuple<string, string>(
                    "john@test.com",
                    "P@ssword!23"
                )
            },
            {
                "Paula", new Tuple<string, string>(
                    "paula@test.com",
                    "P@ssword4%6"
                )
            }
        };

        public static async Task SetRequestAuthorizationToUserAsync(
            this IApiInteractionEngine engine,
            IActiveTokenSession activeTokenSession,
            string userIdentifier = null
        )
        {
            userIdentifier ??= AdminIdentifier;

            var token =
                activeTokenSession
                    .Get(userIdentifier);

            if (string.IsNullOrEmpty(token))
                throw new Exception($"User {userIdentifier} has no active session token");

            await
                engine
                    .ActGetContext()
                    .ActSetRequestHeaderValueAsync(
                        "Authorization",
                        $"Bearer {token}"
                    );
        }

        public static async Task GetBearerTokenForUserAsync(
            this IApiInteractionEngine engine,
            IActiveTokenSession activeTokenSession,
            string userName
        )
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .CreateRequestAsync("/api/auth/login");

            if (!TestUsers.TryGetValue(
                    userName,
                    out var user
                ))
                throw new Exception($"User {userName} not found in test case dictionary");

            await
                context
                    .SetRequestBodyValueAsync(
                        "user_name",
                        user.Item1
                    );

            await
                context
                    .SetRequestBodyValueAsync(
                        "password",
                        user.Item2
                    );

            await
                context
                    .SendRequestAsync("POST");

            await
                context
                    .AssertResponseSucceededAsync();

            var token =
                await
                    context
                        .GetJsonTokenPathValueAsync("access_token");

            activeTokenSession
                .Store(
                    userName,
                    token
                );
        }

        public static async Task SetRequestAuthorizationHeadersAsync(
            this IApiInteractionEngine engine,
            IActiveTokenSession activeTokenSession,
            string userIdentifier
        )
        {
            await
                engine
                    .SetRequestAuthorizationToUserAsync(
                        activeTokenSession,
                        userIdentifier
                    );
        }
    }
}