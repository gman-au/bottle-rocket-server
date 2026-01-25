namespace Rocket.Interfaces
{
    public interface IImageBase64Converter
    {
        string Perform(byte[] imageData);
    }
}