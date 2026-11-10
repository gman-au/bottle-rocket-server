using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.Local.Infrastructure
{
    public class FileWriter : IFileWriter
    {
        public async Task WriteAsync(
            string uploadFilePath,
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            CancellationToken cancellationToken
        )
        {
            var allCompleted = true;
            foreach (var element in elements)
            {
                try
                {
                    var fileName = $"{element.FileName}{element.FileExtension}";

                    var fullFilePath =
                        Path
                            .Combine(
                                uploadFilePath,
                                fileName
                            );

                    await
                        File
                            .WriteAllBytesAsync(
                                fullFilePath,
                                element.Artifact,
                                cancellationToken
                            );
                    
                    await
                        jobControl
                            .SucceedStepAsync(
                                $"File uploaded to local folder: {fullFilePath}",
                                element.ScanId
                            );
                }
                catch (Exception ex)
                {
                    allCompleted = false;
                    
                    await
                        jobControl
                            .FailStepAsync(
                                ex,
                                element.ScanId
                            );
                }
            }

            if (!allCompleted)
            {
                throw new Exception("One or more files failed to upload.");
            }
        }
    }
}