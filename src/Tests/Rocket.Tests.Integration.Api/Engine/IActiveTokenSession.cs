namespace Rocket.Tests.Integration.Api.Engine
{
    public interface IActiveTokenSession
    {
        void Store(
            string userIdentifier,
            string token
        );

        string Get(
            string userIdentifier
        );
    }
}