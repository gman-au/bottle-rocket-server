namespace Rocket.Interfaces
{
    public interface ISha256Calculator
    {
        string CalculateSha256HashAndFormat(byte[] inputBytes);
    }
}