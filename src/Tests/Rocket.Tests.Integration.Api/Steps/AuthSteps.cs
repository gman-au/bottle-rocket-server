using System;
using System.Threading.Tasks;
using Achar.Infrastructure.Api.Extensions;
using Achar.Infrastructure.Testing.Extensions;
using Achar.Interfaces.Testing;
using Reqnroll;
using Rocket.Tests.Integration.Api.Engine;
using Rocket.Tests.Integration.Api.Extensions;

namespace Rocket.Tests.Integration.Api.Steps
{
    [Binding]
    public class AuthSteps(
        IApiInteractionEngine engine,
        IActiveTokenSession activeTokenSession
    )
    {
        [Given("the request authorization is set to the admin user")]
        public async Task GivenTheRequestAuthorizationIsSetToTheAdminUser()
        {
            await
                engine
                    .SetRequestAuthorizationToUserAsync(activeTokenSession);
        }

        [Given("the request authorization is set to the user (.*)")]
        public async Task GivenTheRequestAuthorizationIsSetToTheUser(
            string userIdentifier
        )
        {
            await
                engine
                    .SetRequestAuthorizationHeadersAsync(
                        activeTokenSession,
                        userIdentifier
                    );
        }

        [Given("the user (.*) has been added as an admin")]
        public async Task GivenTheUserHasBeenAddedAsAnAdmin(
            string userIdentifier
        ) =>
            await AddUserAsync(
                userIdentifier,
                true
            );

        [Given("the user (.*) has been added as an admin via user (.*)")]
        public async Task GivenTheUserHasBeenAddedAsAnAdminViaUser(
            string userIdentifier,
            string adminIdentifier
        ) =>
            await AddUserAsync(
                userIdentifier,
                true,
                adminIdentifier
            );

        [Given("the user (.*) has been added as a non-admin")]
        public async Task GivenTheUserHasBeenAddedAsANonAdmin(
            string userIdentifier
        ) =>
            await AddUserAsync(
                userIdentifier,
                false
            );

        [Given("the user (.*) has been added as a non-admin via user (.*)")]
        public async Task GivenTheUserHasBeenAddedAsANonAdminViaUser(
            string userIdentifier,
            string adminIdentifier
        ) =>
            await AddUserAsync(
                userIdentifier,
                false,
                adminIdentifier
            );

        [Given("the user (.*) has a current login session")]
        [When("the user (.*) has a current login session")]
        public async Task GivenTheUserHasACurrentLoginSession(
            string userIdentifier
        ) =>
            await
                engine
                    .GetBearerTokenForUserAsync(
                        activeTokenSession,
                        userIdentifier
                    );

        [Given(@"a prefix ""(.*)"" is added to the value stored in ""(.*)"" and stored under ""(.*)""")]
        [When(@"a prefix ""(.*)"" is added to the value stored in ""(.*)"" and stored under ""(.*)""")]
        public async Task GivenAPrefixIsAddedToTheValueStoredInAndStoredUnder(
            string prependValue,
            string storeKey,
            string newValueStoreKey
        )
        {
            var currentValue =
                await
                    engine
                        .ActGetContext()
                        .ActGetValueStoreValueAsync(
                            storeKey
                        );

            currentValue = prependValue + currentValue;

            await
                engine
                    .ActGetContext()
                    .ActSetValueStoreValueAsync(
                        newValueStoreKey,
                        currentValue
                    );
        }

        private async Task AddUserAsync(
            string userIdentifier,
            bool isAdmin,
            string adminIdentifier = null
        )
        {
            var context =
                await
                    engine
                        .ActGetContext();

            adminIdentifier = adminIdentifier ?? "Admin";

            if (!ApiInteractionEngineEx.TestUsers.TryGetValue(
                    adminIdentifier,
                    out var adminUser
                ))
                throw new Exception($"User {adminIdentifier} not found in test case dictionary");

            await
                context
                    .CreateRequestAsync("/api/users/create");

            await
                engine
                    .SetRequestAuthorizationToUserAsync(
                        activeTokenSession,
                        adminIdentifier
                    );

            if (!ApiInteractionEngineEx.TestUsers.TryGetValue(
                    userIdentifier,
                    out var user
                ))
                throw new Exception($"User {userIdentifier} not found in test case dictionary");

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
                    .SetRequestBodyValueAsync(
                        "is_the_new_admin",
                        isAdmin
                    );

            await
                context
                    .SendRequestAsync("POST");

            await
                context
                    .AssertResponseSucceededAsync();
        }
    }
}