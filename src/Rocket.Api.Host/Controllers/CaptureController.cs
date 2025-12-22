using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain;
using Rocket.Domain.Enum;
using Rocket.Interfaces;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/capture")]
    public class CaptureController(
        IScannedImageHandler scannedImageHandler,
        ILogger<CaptureController> logger
    ) : ControllerBase
    {
        [HttpPost("process")]
        public async Task<IActionResult> ProcessCaptureAsync(
            [FromForm] ImageUploadModel model,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received capture");

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

                await
                    formFile
                        .CopyToAsync(
                            ms,
                            cancellationToken
                        );

                await
                    scannedImageHandler
                        .HandleAsync(
                            ms.ToArray(),
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
        }
    }
}