using System.Collections.Generic;

namespace Rocket.Tests.Integration.Api.Engine
{
    public class ActiveTokenSession : IActiveTokenSession
    {
        private readonly Dictionary<string, string> _tokens = new();

        public void Store(
            string userIdentifier,
            string token
        )
        {
            _tokens
                .TryAdd(
                    userIdentifier,
                    token
                );
        }

        public string Get(
            string userIdentifier
        )
        {
            return
                _tokens
                    .TryGetValue(
                        userIdentifier,
                        out var token
                    )
                    ? token
                    : null;
        }
    }
}