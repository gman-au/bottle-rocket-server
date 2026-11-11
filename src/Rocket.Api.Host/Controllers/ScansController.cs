using System;
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
        IUserManager userManager,
        ICaptureNotifier captureNotifier,
        IScannedImageHandler scannedImageHandler,
        IDashboardSnapshotProvider dashboardSnapshotProvider
    ) : RocketControllerBase(userManager)
    {
        [HttpGet("{batchId}/{scanId}")]
        [EndpointSummary("Fetch the user scan details")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Retrieves the full details of a users scan by its unique ID and batch ID.\n
            The full image data is provided as well as other capture and processing details. 
            """
        )]
        [ProducesResponseType(
            typeof(ScanSpecifics),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchMyScanAsync(
            string batchId,
            string scanId,
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
                    scanId
                );

            var (record, imageData) =
                await
                    scannedImageHandler
                        .ReadFromBatchAsync(
                            userId,
                            batchId,
                            scanId,
                            cancellationToken
                        );

            var response =
                new ScanSpecifics
                {
                    Id = record.Id,
                    Description = record.Description,
                    UserId = record.UserId,
                    CaptureDate = record.CaptureDate.ToLocalTime(),
                    BlobId = record.BlobId,
                    ContentType = record.ContentType,
                    FileExtension = record.FileExtension,
                    Sha256 = record.Sha256,
                    ImageBase64 = Convert.ToBase64String(imageData),
                    Vendor = record.Vendor
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("delete/{batchId}/{scanId}")]
        [EndpointSummary("(Permanently) delete a scan")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Permanently and completely deletes a scan by its unique ID.\n
            All traces of the scan, including prior executions, will be removed. The image file is deleted from the working folder.
            """
        )]
        [ProducesResponseType(
            typeof(DeleteScanResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteScanAsync(
            string batchId,
            string scanId,
            CancellationToken cancellationToken
        )
        {
            var user =
                await
                    ThrowIfNotActiveUserAsync(cancellationToken);

            var userId =
                user
                    .Id;

            logger
                .LogInformation(
                    "Received (manual) deletion request for username: {userId}, id: {id}",
                    userId,
                    scanId
                );

            await
                scannedImageHandler
                    .DeleteFromBatchAsync(
                        userId,
                        batchId,
                        scanId,
                        cancellationToken
                    );

            var response =
                new DeleteScanResponse();

            dashboardSnapshotProvider
                .MarkAsDirty(userId);

            await
                captureNotifier
                    .NotifyScanDeleteAsync(
                        userId,
                        cancellationToken
                    );

            return
                response
                    .AsApiSuccess();
        }
    }
}