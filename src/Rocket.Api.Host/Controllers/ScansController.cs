using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/scans")]
    [Authorize]
    public class ScansController(
        ILogger<ScansController> logger,
        IScannedImageRepository scannedImageRepository
    ) : ControllerBase
    {
        [HttpPost("fetch")]
        public async Task<IActionResult> FetchMyScansAsync(
            [FromBody] MyScansRequest request,
            CancellationToken cancellationToken
        )
        {
            var userId =
                User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                throw new RocketException(
                    "User ID not found in claims",
                    ApiStatusCodeEnum.UnknownUser,
                    (int)HttpStatusCode.Unauthorized
                );

            logger
                .LogInformation(
                    "Received fetch scans request for username: {userId}",
                    userId
                );


            var (records, totalRecordCount) =
                await
                    scannedImageRepository
                        .SearchScansAsync(
                            userId,
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new MyScansResponse
                {
                    Scans =
                        records
                            .Select(
                                o =>
                                    new Scan
                                    {
                                        Id = o.Id,
                                        DateScanned = o.CaptureDate.ToLocalTime(),
                                        ThumbnailBase64 = o.ThumbnailBase64,
                                        ContentType = o.ContentType
                                    }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}