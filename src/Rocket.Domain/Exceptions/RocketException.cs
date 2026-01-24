using Rocket.Domain.Core.Enum;

namespace Rocket.Domain.Exceptions
{
    public class RocketException(
        string message = "There was an error.",
        int apiStatusCode = (int)ApiStatusCodeEnum.UnknownError,
        int httpStatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
        System.Exception innerException = null
    )
        : System.Exception(
            message,
            innerException
        )
    {
        public readonly int ApiStatusCode = apiStatusCode;
        
        public readonly int HttpStatusCode = httpStatusCode;

        public RocketException(
            string message = "There was an error.",
            ApiStatusCodeEnum apiStatusCode = ApiStatusCodeEnum.UnknownError,
            int httpStatusCode = (int)System.Net.HttpStatusCode.InternalServerError,
            System.Exception innerException = null
        ) : this(
            message,
            (int)apiStatusCode,
            httpStatusCode,
            innerException
        )
        {
        }

        public RocketException() : this(
            null,
            ApiStatusCodeEnum.UnknownError
        )
        {
        }
    }
}