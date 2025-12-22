using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocket.Api.Contracts;
using Rocket.Api.Host.Extensions;
using Rocket.Domain;
using Rocket.Domain.Enum;

namespace Rocket.Api.Host.Controllers
{
    [ApiController]
    [Route("/api/capture")]
    public class CaptureController(ILogger<CaptureController> logger) : ControllerBase
    {
        [HttpPost("process")]
        public IActionResult GetHealthCheck(
            [FromForm] ImageUploadModel model,
            CancellationToken cancellationToken
        )
        {
            logger
                .LogInformation("Received capture");

            using var ms = new MemoryStream();

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