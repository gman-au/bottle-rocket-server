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
        [Given("the request authorization is set to the admin user")]
        public async Task GivenTheRequestAuthorizationIsSetToTheAdminUser()
        {
            const string userName = "admin";
            const string password = "password123";

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