using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Scans;
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
        IScannedImageHandler scannedImageHandler,
        IScannedImageRepository scannedImageRepository
    ) : ControllerBase
    {
        [HttpPost("fetch")]
        [EndpointSummary("Fetch the users scans")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Retrieves a subset of scans belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load.\n 
            A base64 thumbnail image string is also provided instead of the full image data in a multi-record payload.
            """
        )]
        [ProducesResponseType(typeof(MyScansResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                        .FetchScansAsync(
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
                                    new MyScanItem
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

        [HttpGet("{id}")]
        [EndpointSummary("Fetch the user scan details")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Retrieves the full details of a users scan by its unique ID.\n
            The full image data is provided as well as other capture and processing details. 
            """
        )]
        [ProducesResponseType(typeof(MyScanItemDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchMyScanAsync(
            string id,
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
                    "Received fetch scan request for username: {userId}, id: {id}",
                    userId,
                    id
                );


            var (record, imageData) =
                await
                    scannedImageHandler
                        .ReadAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            var response =
                new MyScanItemDetail
                {
                    Id = record.Id,
                    UserId = record.UserId,
                    CaptureDate = record.CaptureDate.ToLocalTime(),
                    BlobId = record.BlobId,
                    ContentType = record.ContentType,
                    FileExtension = record.FileExtension,
                    Sha256 = record.Sha256,
                    ImageBase64 = Convert.ToBase64String(imageData)
                };

            return
                response
                    .AsApiSuccess();
        }
    }
}