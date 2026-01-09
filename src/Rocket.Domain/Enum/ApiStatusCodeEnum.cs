namespace Rocket.Domain.Enum
{
    public enum ApiStatusCodeEnum
    {
        Ok = 0,
        UnknownError = 1000,
        NoAttachmentsFound,
        ServerConnectionError,
        UserAlreadyExists = 2000,
        UnknownUser,
        InactiveUser,
        InvalidUsername,
        InvalidPassword,
        FileDataCorrupted,
        UnknownOrInaccessibleRecord,
        RequiresAdministratorAccess,
        PotentiallyIrrecoverableOperation,
        ServerError = 5000
    }
}