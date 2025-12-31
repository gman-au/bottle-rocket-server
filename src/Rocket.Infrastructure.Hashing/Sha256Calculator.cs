using System;
using System.Security.Cryptography;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Hashing
{
    public class Sha256Calculator : ISha256Calculator
    {
        public string CalculateSha256HashAndFormat(byte[] inputBytes)
        {
            var hashBytes = 
                SHA256
                    .HashData(inputBytes);

            var hashString = 
                Convert
                    .ToHexString(hashBytes);
        
            return 
                hashString
                    .ToLowerInvariant();
        }
    }
}