using System.Threading.Tasks;
using Achar.Infrastructure.Testing.Extensions;
using Reqnroll;
using Rocket.Tests.Integration.Api.Engine;

namespace Rocket.Tests.Integration.Api.Steps
{
    [Binding]
    public class CaptureSteps(IApiExtendedInteractionEngine engine)
    {
        [Given("the extended method is called against endpoint \"(.*)\"")]
        public async Task ExtendedMethodIsCalled(string endpoint)
        {
            var context =
                await
                    engine
                        .ActGetContext();

            await
                context
                    .SendMultiPartRequestAsync(
                        "POST",
                        endpoint,
                        new byte[0],
                        "image/png",
                        "image.png"
                    );
        }
    }
}