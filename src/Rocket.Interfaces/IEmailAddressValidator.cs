namespace Rocket.Interfaces
{
    public interface IEmailAddressValidator
    {
        bool IsValid(string emailAddress);
    }
}