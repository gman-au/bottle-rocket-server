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
using Rocket.Api.Contracts.Batches;
using Rocket.Api.Contracts.Scans;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Batches;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/batches")]
    [Authorize]
    public class BatchController(
        ILogger<BatchController> logger,
        IUserManager userManager,
        ICaptureNotifier captureNotifier,
        IBatchHandler batchHandler,
        IBatchRepository batchRepository,
        IDashboardSnapshotProvider dashboardSnapshotProvider
    ) : RocketControllerBase(userManager)
    {
        [HttpGet("open")]
        [EndpointSummary("Open a new batch")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Opens a new batch for scanning and returns the batch ID.
            """
        )]
        [ProducesResponseType(
            typeof(OpenBatchResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> OpenBatchAsync(
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
                    "Received open batch request for username: {userId}",
                    userId
                );

            var batch = new Batch
            {
                UserId = userId,
                CaptureDate = DateTime.UtcNow,
                Status = (int)BatchStatusEnum.Open,
                Scans = []
            };

            batch =
                await
                    batchRepository
                        .InsertBatchAsync(
                            batch,
                            cancellationToken
                        );

            var response =
                new OpenBatchResponse
                {
                    Id = batch.Id
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("close")]
        [EndpointSummary("Close a batch")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Closes an (open) batch./nThe user can also opt to run one or more workflows against the entire batch.
            """
        )]
        [ProducesResponseType(
            typeof(CloseBatchResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CloseBatchAsync(
            [FromBody] CloseBatchRequest request,
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
                    "Received close batche request for username: {userId}",
                    userId
                );

            await
                batchRepository
                    .UpdateBatchFieldAsync(
                        userId,
                        request.Id,
                        o => o.Status,
                        (int)BatchStatusEnum.Closed,
                        cancellationToken
                    );

            var response =
                new CloseBatchResponse
                {
                    IsStarted = false
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpPost("fetch")]
        [EndpointSummary("Fetch the users batches")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Retrieves a subset of scans belonging to the authenticated user.\n
            Provide a zero-based start index and record count to retrieve paged results and minimise server load.\n 
            A base64 thumbnail image string is also provided instead of the full image data in a multi-record payload.
            """
        )]
        [ProducesResponseType(
            typeof(MyBatchesResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> FetchMyBatchesAsync(
            [FromBody] MyBatchesRequest request,
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
                    "Received fetch batches request for username: {userId}",
                    userId
                );


            var (records, totalRecordCount) =
                await
                    batchRepository
                        .FetchBatchesAsync(
                            userId,
                            request.StartIndex,
                            request.RecordCount,
                            cancellationToken
                        );

            var response =
                new MyBatchesResponse
                {
                    Batches =
                        records
                            .Select(o =>
                                new BatchSummary
                                {
                                    Id = o.Id,
                                    DateScanned = o.CaptureDate.ToLocalTime(),
                                    Scans =
                                        o
                                            .Scans
                                            .Select(e =>
                                                new ScanSummary
                                                {
                                                    Id = e.Id,
                                                    Description = e.Description,
                                                    DateScanned = e.CaptureDate,
                                                    ContentType = e.ContentType,
                                                    ThumbnailBase64 = e.ThumbnailBase64
                                                }
                                            )
                                }
                            ),
                    TotalRecords = (int)totalRecordCount
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpGet("{id}")]
        [EndpointSummary("Fetch the user batch details")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Retrieves the full details of a users batch by its unique ID.\n
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
        public async Task<IActionResult> FetchMyBatchAsync(
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
                    "Received fetch batch request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            // we still don't need to fetch the full image data just yet
            var record =
                await
                    batchRepository
                        .GetBatchByIdAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            // take the top 3 page thumbnails (the UI will collage them)
            var response =
                new BatchSpecifics
                {
                    Id = record.Id,
                    DateScanned = record.CaptureDate.ToLocalTime(),
                    Status = record.Status,
                    Scans =
                        record
                            .Scans
                            .Select(o =>
                                new ScanSummary
                                {
                                    Id = o.Id,
                                    Description = o.Description,
                                    DateScanned = o.CaptureDate,
                                    ContentType = o.ContentType,
                                    ThumbnailBase64 = o.ThumbnailBase64
                                }
                            )
                            .Take(3),
                    ScanCount =
                        record
                            .Scans
                            .Count()
                };

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("archive/{id}")]
        [EndpointSummary("Archive a batch")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Manually archives a batch by its unique ID.\n
            To preserve drive space, the original image files are permanently deleted and will no longer be available
            for workflow processing. A stubbed record will remain with a small thumbnail image set for reference.
            """
        )]
        [ProducesResponseType(
            typeof(ArchiveBatchResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ArchiveBatchAsync(
            string id,
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
                    "Received (manual) batch archival request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            var result =
                await
                    batchHandler
                        .ArchiveAsync(
                            userId,
                            id,
                            cancellationToken
                        );

            var response =
                new ArchiveScanResponse
                {
                    IsArchived = result
                };

            dashboardSnapshotProvider
                .MarkAsDirty(userId);

            return
                response
                    .AsApiSuccess();
        }

        [HttpDelete("delete/{id}")]
        [EndpointSummary("(Permanently) delete a batch")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription(
            """
            Permanently and completely deletes a batch by its unique ID.\n
            All traces of the batch, including prior executions, will be removed. The image files are deleted from the working folder.
            """
        )]
        [ProducesResponseType(
            typeof(DeleteBatchResponse),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(
            typeof(ApiResponse),
            StatusCodes.Status500InternalServerError
        )]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteBatchAsync(
            string id,
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
                    "Received (manual) batch deletion request for username: {userId}, id: {id}",
                    userId,
                    id
                );

            await
                batchHandler
                    .DeleteAsync(
                        userId,
                        id,
                        cancellationToken
                    );

            var response =
                new DeleteBatchResponse();

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