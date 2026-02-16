using Reqnroll;

namespace Rocket.Tests.Integration.Api.Steps
{
    [Binding]
    public class BasicSteps
    {
        [Given("this is starting up")]
        public void GivenThisIsStartingUp()
        {
            ScenarioContext.StepIsPending();
        }
    }
}