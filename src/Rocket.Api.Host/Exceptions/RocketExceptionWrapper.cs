using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Domain;

namespace Rocket.Api.Host.Exceptions
{
    public class RocketExceptionWrapper(ILoggerFactory loggerFactory) : IRocketExceptionWrapper
    {
        public ObjectResult For(System.Exception exception)
        {
            var logger =
                loggerFactory
                    .CreateLogger<RocketExceptionWrapper>();

            logger
                .LogError(
                    exception,
                    exception.Message
                );

            var rocketException = exception as RocketException ?? new RocketException();

            var contextResult =
                new ObjectResult(
                    new ApiResponse
                    {
                        ErrorCode = rocketException.ApiStatusCode,
                        ErrorMessage = rocketException.Message
                    }
                )
                {
                    StatusCode = rocketException.HttpStatusCode
                };

            return contextResult;
        }
    }
}