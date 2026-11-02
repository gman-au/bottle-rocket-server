using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Auth
{
    public class JwtTokenGenerator
        : IJwtTokenGenerator, IAppHealthCheck
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

        public async Task PreRunAsync()
        {
            try
            {
                var token =
                    new JwtSecurityToken(
                        claims: [],
                        notBefore: DateTime.UtcNow,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: _credentials
                    );

                new JwtSecurityTokenHandler()
                    .WriteToken(token);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new Exception(
                    "The JWT signing key configured is of insufficient size. Ensure that the configured value for JWT_AUTH_SECRET is at least 32 bytes long.",
                    ex
                );
            }
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