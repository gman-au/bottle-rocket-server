using System.Linq;
using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace Rocket.Tests.Unit.Extensions
{
    public static class FixtureEx
    {
        public static IFixture CreateNSubstituteFixture()
        {
            var fixture =
                new Fixture()
                    .Customize(new AutoNSubstituteCustomization());

            fixture
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            fixture
                .Behaviors
                .Add(new OmitOnRecursionBehavior());
            
            return fixture;
        }
    }
}