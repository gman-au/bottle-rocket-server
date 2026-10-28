using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class AuthSecretProvider : IAuthSecretProvider
    {
        private readonly string _authSecret;

        public AuthSecretProvider(
            IConfiguration configuration
        )
        {
            _authSecret =
                configuration["JWT_AUTH_SECRET"];

            if (!string.IsNullOrEmpty(_authSecret)) return;
            
            var stableInput = $"{Environment.MachineName}-{Environment.ProcessId}";
            
            var inputBytes = 
                Encoding
                    .UTF8
                    .GetBytes(stableInput);
            
            var hashBytes = 
                SHA256
                    .HashData(inputBytes);

            _authSecret = 
                Convert
                    .ToHexString(hashBytes);
        }

        public string Get() => _authSecret;
    }
}