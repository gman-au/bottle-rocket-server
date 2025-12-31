namespace Rocket.Interfaces
{
    public interface IPasswordGenerator
    {
        string GenerateRandomPassword(int passwordLength = 16);
    }
}