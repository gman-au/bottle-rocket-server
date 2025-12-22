using Microsoft.AspNetCore.Mvc;
using Rocket.Api.Contracts;
using Rocket.Domain.Enum;

namespace Rocket.Api.Host.Extensions
{
    internal static class ApiResponseEx
    {
        public static IActionResult AsApiSuccess<T>(this T apiResponse) where T : ApiResponse
        {
            apiResponse.ErrorMessage = "OK";
            apiResponse.ErrorCode = (int)ApiStatusCodeEnum.Ok;
            return new OkObjectResult(apiResponse);
        }
    }
}