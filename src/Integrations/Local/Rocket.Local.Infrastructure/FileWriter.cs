using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Enum;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Domain;
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
                            .AppendLogMessageCallback(
                                new JobLogEntry
                                {
                                    ScanId = element.ScanId,
                                    LogMessage = $"File uploaded: {element.FileName}{element.FileExtension}"
                                }
                            );

                    await
                        jobControl
                            .UpdateStepStatusCallback(
                                new JobStatusEntry
                                {
                                    ExecutionStatus = (int)ExecutionStatusEnum.Completed,
                                    ScanId = element.ScanId,
                                    Exception = null
                                }
                            );
                }
                catch (Exception ex)
                {
                    allCompleted = false;
                    
                    await
                        jobControl
                            .UpdateStepStatusCallback(
                                new JobStatusEntry
                                {
                                    ExecutionStatus = (int)ExecutionStatusEnum.Errored,
                                    ScanId = element.ScanId,
                                    Exception = ex
                                }
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