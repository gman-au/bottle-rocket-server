using System;
using System.Threading.Tasks;
using Reqnroll;

namespace Rocket.Tests.Integration.Api.Steps
{
    [Binding]
    public class IntervalSteps
    {
        [Given("(.*) seconds have passed")]
        [When("(.*) seconds have passed")]
        [Then("(.*) seconds have passed")]
        public async Task GivenSecondsHavePassed(
            int seconds
        )
        {
            await
                Task
                    .Delay(TimeSpan.FromSeconds(seconds));
        }
    }
}