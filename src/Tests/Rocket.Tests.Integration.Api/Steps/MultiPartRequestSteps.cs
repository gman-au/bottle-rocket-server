using System.Threading.Tasks;
using Achar.Infrastructure.Testing.Extensions;
using Reqnroll;
using Rocket.Tests.Integration.Api.Engine;

namespace Rocket.Tests.Integration.Api.Steps
{
    [Binding]
    public class MultiPartRequestSteps(IApiExtendedInteractionEngine engine)
    {
        [Given("the multipart file data is set to base64 string \"(.*)\"")]
        public Task GivenTheMultipartFileDataIsSetToBaseString(string imageBase64)
        {
            engine
                .SetImageBase64(imageBase64);

            return
                Task
                    .CompletedTask;
        }

        [Given(@"an API multipart request is created against endpoint ""(.*)""")]
        public async Task GivenAnApiMultipartRequestIsCreatedAgainstEndpoint(string endpoint)
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .CreateRequestAsync(endpoint);
        }

        [When("the multipart request is sent via \"(.*)\"")]
        public async Task WhenTheMultipartRequestIsSentVia(string method)
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .SendMultiPartRequestAsync(
                        method,
                        "image/png",
                        "image.png"
                    );
        }

        [Then(@"the multipart request should have failed with status code (.*)")]
        public async Task ThenTheMultipartRequestShouldHaveFailedWithStatusCode(int expectedStatusCode)
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .AssertResponseFailedAsync(expectedStatusCode);
        }

        [Then(@"the multipart response with path ""(.*)"" should have a value of ""(.*)""")]
        public async Task ThenTheMultipartResponseWithPathShouldHaveAValueOf(
            string path,
            string expectedValue
        )
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .AssertJsonTokenPathValueEqualsAsync(
                        path,
                        expectedValue
                    );
        }

        [Then(@"the multipart request should have succeeded")]
        public async Task ThenTheMultipartRequestShouldHaveSucceeded()
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .AssertResponseSucceededAsync();
        }
    }
}