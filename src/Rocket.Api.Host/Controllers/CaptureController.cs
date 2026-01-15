using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Contracts.Captures;
using Rocket.Api.Contracts.Scans;
using Rocket.Api.Host.Extensions;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/capture")]
    public class CaptureController(
        IScannedImageHandler scannedImageHandler,
        ICaptureNotifier captureNotifier,
        ILogger<CaptureController> logger
    ) : ControllerBase
    {
        [HttpPost("process")]
        [EndpointSummary("Process captured image(s)")]
        [EndpointGroupName("Manage captures / scans")]
        [EndpointDescription("Process uploaded images via this endpoint. Use the url-encoded multipart form schema to POST the image data.")]
        [ProducesResponseType(typeof(ProcessCaptureResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ProcessCaptureAsync(
            [FromForm] ImageUploadModel model,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received capture");

            var userId =
                User
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                logger
                    .LogWarning("User authentication information missing from claims");
                
                throw new RocketException(
                    "User authentication failed",
                    ApiStatusCodeEnum.UnknownUser,
                    401
                );
            }
            
            var form =
                model?
                    .Form;

            var formFiles =
                form?
                    .Files;

            if ((formFiles?.Count ?? 0) == 0)
                throw new RocketException(
                    "No file attachments found.",
                    ApiStatusCodeEnum.NoAttachmentsFound
                );

            foreach (var formFile in formFiles)
            {
                using var ms = new MemoryStream();

                var contentType = 
                    formFile
                        .ContentType;

                var fileExtension =
                    Path
                        .GetExtension(formFile.FileName);
                
                await
                    formFile
                        .CopyToAsync(
                            ms,
                            cancellationToken
                        );

                await
                    scannedImageHandler
                        .WriteAsync(
                            ms.ToArray(),
                            contentType,
                            fileExtension,
                            userId,
                            model.QrCode,
                            model.QrBoundingBox,
                            cancellationToken
                        );

                await
                    captureNotifier
                        .NotifyNewCaptureAsync(
                            userId,
                            cancellationToken
                        );
            }
            
            return
                new ProcessCaptureResponse()
                    .AsApiSuccess();
        }

        public class ImageUploadModel
        {
            public IFormCollection Form { get; set; }
            
            [FromForm(Name = "qr_code")]
            public string QrCode { get; set; }
            
            [FromForm(Name = "qr_bounding_box")]
            public string QrBoundingBox { get; set; }
        }
    }
}