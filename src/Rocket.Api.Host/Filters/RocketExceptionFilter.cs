using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Infrastructure.Auth;
using Rocket.Infrastructure.Auth.Exceptions;

namespace Rocket.Api.Host.Filters
{
    public class RocketExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception =
                context
                    .Exception;

            var exceptionWrapper =
                context
                    .HttpContext
                    .RequestServices
                    .GetService<IRocketExceptionWrapper>();

            context.Result =
                exceptionWrapper
                    .For(exception);
        }
    }
}