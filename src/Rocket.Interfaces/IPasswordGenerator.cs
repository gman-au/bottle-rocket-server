namespace Rocket.Interfaces
{
    public interface IPasswordGenerator
    {
        string GeneratePassword(int passwordLength = 16);
    }
}