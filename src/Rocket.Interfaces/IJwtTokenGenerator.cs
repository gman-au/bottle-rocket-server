using System;

namespace Rocket.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string CreateAccessToken(
            string userId,
            string sessionId,
            bool isAdmin,
            DateTime expiresUtc
        );
    }
}