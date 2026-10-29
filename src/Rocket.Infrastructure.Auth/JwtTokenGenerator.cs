using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Auth
{
    public class JwtTokenGenerator
        : IJwtTokenGenerator
    {
        private readonly SigningCredentials _credentials;

        public JwtTokenGenerator(
            IAuthSecretProvider authSecretProvider
        )
        {
            var key =
                Encoding
                    .ASCII
                    .GetBytes(
                        authSecretProvider.Get()
                    );

            _credentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                );
        }

        public string CreateAccessToken(
            string userId,
            string sessionId,
            bool isAdmin,
            DateTime expiresUtc
        )
        {
            var claims =
                new List<Claim>
                {
                    new(
                        ClaimTypes.NameIdentifier,
                        userId
                    ),
                    new(
                        AuthConstants.SessionId,
                        sessionId
                    ),
                };

            if (isAdmin)
                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        "Admin"
                    )
                );

            var token =
                new JwtSecurityToken(
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiresUtc,
                    signingCredentials: _credentials
                );

            return
                new JwtSecurityTokenHandler()
                    .WriteToken(token);
        }
    }
}