using Microsoft.AspNetCore.Mvc;

namespace Rocket.Api.Host.Exceptions
{
    public interface IRocketExceptionWrapper
    {
        ObjectResult For(System.Exception exception);
    }
}