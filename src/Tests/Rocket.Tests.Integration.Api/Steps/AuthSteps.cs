using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Achar.Infrastructure.Api.Extensions;
using Achar.Infrastructure.Testing.Extensions;
using Achar.Interfaces.Testing;
using Reqnroll;

namespace Rocket.Tests.Integration.Api.Steps
{
    [Binding]
    public class AuthSteps(IApiInteractionEngine engine)
    {
        private const string AdminUserName = "admin";
        private const string AdminPassword = "password123";

        private readonly Dictionary<string, Tuple<string, string>> _testUsers = new()
        {
            { "Admin", new Tuple<string, string>(AdminUserName, AdminPassword) },
            { "John", new Tuple<string, string>("john@test.com", "P@ssword!23") },
            { "Paula", new Tuple<string, string>("paula@test.com", "P@ssword4%6") }
        };

        [Given("the request authorization is set to the admin user")]
        public async Task GivenTheRequestAuthorizationIsSetToTheAdminUser()
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                SetRequestAuthorizationToAdminUserAsync(context);
        }

        [Given("the request authorization is set to the user (.*)")]
        public async Task GivenTheRequestAuthorizationIsSetToTheUser(string userIdentifier)
        {
            var context =
                await
                    engine
                        .ActGetContext();

            if (!_testUsers.TryGetValue(userIdentifier, out var user))
                throw new Exception($"User {userIdentifier} not found in test case dictionary");

            await
                SetRequestAuthorizationToAdminUserAsync(
                    context,
                    user.Item1,
                    user.Item2
                );
        }

        [Given("the user (.*) has been added as an admin")]
        public async Task GivenTheUserHasBeenAddedAsAnAdmin(string userIdentifier)
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .CreateRequestAsync("/api/users/create");

            await
                SetRequestAuthorizationToAdminUserAsync(context);

            if (!_testUsers.TryGetValue(userIdentifier, out var user))
                throw new Exception($"User {userIdentifier} not found in test case dictionary");

            await
                context
                    .SetRequestBodyValueAsync("user_name", user.Item1);

            await
                context
                    .SetRequestBodyValueAsync("password", user.Item2);

            await
                context
                    .SetRequestBodyValueAsync("is_the_new_admin", true);

            await
                context
                    .SendRequestAsync("POST");

            await
                context
                    .AssertResponseSucceededAsync();
        }

        private static async Task SetRequestAuthorizationToAdminUserAsync(
            IApiInteractionEngine engine,
            string userName = null,
            string password = null
        )
        {
            userName ??= AdminUserName;
            password ??= AdminPassword;

            var bytes =
                Encoding
                    .Default
                    .GetBytes(userName + ":" + password);

            var basicAuth =
                Convert
                    .ToBase64String(bytes);

            await
                engine
                    .ActGetContext()
                    .ActSetRequestHeaderValueAsync("Authorization", $"Basic {basicAuth}");
        }
    }
}