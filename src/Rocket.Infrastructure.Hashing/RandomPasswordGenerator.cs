using System.Linq;
using System.Security.Cryptography;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.Hashing
{
    public class RandomPasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword(int passwordLength = 16)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";

            var randomBytes = new byte[passwordLength];
            
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var chars = 
                randomBytes
                    .Select(b => validChars[b % validChars.Length])
                    .ToArray();
            
            return new string(chars);
        } 
    }
}