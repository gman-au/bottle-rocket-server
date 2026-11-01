using Microsoft.AspNetCore.Mvc;

namespace Rocket.Infrastructure.Auth.Exceptions
{
    public interface IRocketExceptionWrapper
    {
        ObjectResult For(System.Exception exception);
    }
}