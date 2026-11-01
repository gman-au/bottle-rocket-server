namespace Rocket.Interfaces
{
    public interface IUserAgentParser
    {
        string BuildDeviceName(
            string userAgent
        );
    }
}