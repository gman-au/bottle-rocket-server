using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace Rocket.Web.Host.Extensions
{
    internal static class BrowserFileEx
    {
        private const long MaxUploadSizeMb = 5;
        private const long MaxUploadSize = MaxUploadSizeMb * 1000000;

        public static async Task<string> ConvertToBase64Async(
            this IBrowserFile file,
            CancellationToken cancellationToken
        )
        {
            var stream =
                file
                    .OpenReadStream(
                        MaxUploadSize,
                        cancellationToken
                    );

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                await
                    stream
                        .CopyToAsync(
                            memoryStream,
                            cancellationToken
                        );

                bytes = memoryStream.ToArray();
            }

            var base64 =
                Convert
                    .ToBase64String(bytes);

            return base64;
        }
    }
}