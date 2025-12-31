using Rocket.Interfaces;

namespace Rocket.Infrastructure.Hashing
{
    public class StaticInsecurePasswordGenerator : IPasswordGenerator
    {
        public string GeneratePassword(int passwordLength = 16)
        {
            return "password123";
        } 
    }
}