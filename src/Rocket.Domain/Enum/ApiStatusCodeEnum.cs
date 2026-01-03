namespace Rocket.Domain.Enum
{
    public enum ApiStatusCodeEnum
    {
        Ok = 0,
        UnknownError = 1000,
        NoAttachmentsFound,
        UserAlreadyExists = 2000,
        UnknownUser,
        InvalidUsername,
        InvalidPassword,
        FileDataCorrupted
    }
}