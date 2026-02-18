using System;
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

        private const string TestUserName = "user@test.com";
        private const string TestPassword = "P@ssword!23";

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

        [Given("the request authorization is set to the test user")]
        public async Task GivenTheRequestAuthorizationIsSetToTheTestUser()
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                SetRequestAuthorizationToAdminUserAsync(
                    context,
                    TestUserName,
                    TestPassword
                );
        }

        [Given("the test user has been added as an admin")]
        public async Task GivenTheTestUserHasBeenAddedAsAnAdmin()
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

            await
                context
                    .SetRequestBodyValueAsync("user_name", TestUserName);

            await
                context
                    .SetRequestBodyValueAsync("password", TestPassword);

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