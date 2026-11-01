using Rocket.Interfaces;
using UAParser;

namespace Rocket.Web.Infrastructure.Auth
{
    public class UserAgentParser : IUserAgentParser
    {
        public string BuildDeviceName(
            string userAgent
        )
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return "Unknown Device";

            var uaParser = Parser.GetDefault();

            var client =
                uaParser
                    .Parse(userAgent);

            var device =
                client
                    .Device
                    .Family?
                    .Trim();

            var os =
                client
                    .OS
                    .Family?
                    .Trim();

            return
                string
                    .Join(
                        ", ",
                        client,
                        os,
                        device
                    );
        }
    }
}