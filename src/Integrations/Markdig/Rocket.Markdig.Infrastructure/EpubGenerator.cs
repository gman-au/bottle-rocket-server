using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;
using Rocket.Markdig.Infrastructure.Definition;

namespace Rocket.Markdig.Infrastructure
{
    public class EpubGenerator(
        IEpubRenderer epubRenderer
    ) : IEpubGenerator
    {
        private const string DefaultEnglishLanguage = "en";

        public async Task<IEnumerable<ArtifactDataElement>> GenerateEpubFromTextAsync(
            IJobControl jobControl,
            ExecutionStepArtifact artifact,
            bool mergeFiles,
            string author,
            CancellationToken cancellationToken
        )
        {
            var elements =
                (artifact
                    .Elements ?? [])
                    .ToList();
            
            var results = new List<ArtifactDataElement>();
            string scanId = null;

            if (mergeFiles)
            {
                try
                {
                    var title = 
                        artifact
                            .FileName;

                    var metadata =
                        new Metadata
                        {
                            Author = author,
                            Title = title
                        };

                    var chapters = new List<Chapter>();

                    foreach (var element in elements)
                    {
                        try
                        {
                            scanId = 
                                element
                                    .ScanId;

                            await
                                jobControl
                                    .StartStepAsync(
                                        scanId
                                    );

                            var chapter =
                                new Chapter(
                                    element.FileName,
                                    Encoding
                                        .Default
                                        .GetString(element.Artifact)
                                );

                            chapters
                                .Add(chapter);

                            await
                                jobControl
                                    .SucceedStepAsync(
                                        "Text converted to merged EPUB successfully",
                                        scanId
                                    );
                        }
                        catch (Exception ex)
                        {
                            await
                                jobControl
                                    .FailStepAsync(
                                        ex,
                                        scanId
                                    );
                        }
                    }

                    var epubArtifact =
                        await
                            epubRenderer
                                .RenderToStreamAsync(
                                    metadata,
                                    chapters,
                                    DefaultEnglishLanguage,
                                    cancellationToken
                                );

                    results
                        .Add(
                            new ArtifactDataElement
                            {
                                Artifact = epubArtifact,
                                FileExtension = CommonFileConstants.EpubExtension,
                                ScanId = null,
                                FileName = artifact.FileName
                            }
                        );

                    await
                        jobControl
                            .SucceedStepAsync(
                                "Text converted to EPUB successfully"
                            );
                }
                catch (Exception ex)
                {
                    await
                        jobControl
                            .FailStepAsync(
                                ex
                            );
                }
            }
            else
            {
                foreach (var element in elements)
                {
                    scanId = 
                        element
                            .ScanId;

                    await
                        jobControl
                            .StartStepAsync(
                                scanId
                            );

                    try
                    {
                        var title = element.FileName;

                        var metadata =
                            new Metadata
                            {
                                Author = author,
                                Title = title
                            };

                        var chapter =
                            new Chapter(
                                title,
                                Encoding
                                    .Default
                                    .GetString(element.Artifact)
                            );

                        var epubArtifact =
                            await
                                epubRenderer
                                    .RenderToStreamAsync(
                                        metadata,
                                        new List<Chapter> { chapter },
                                        DefaultEnglishLanguage,
                                        cancellationToken
                                    );

                        results
                            .Add(
                                new ArtifactDataElement
                                {
                                    Artifact = epubArtifact,
                                    FileExtension = CommonFileConstants.EpubExtension,
                                    ScanId = null,
                                    FileName = element.FileName
                                }
                            );

                        await
                            jobControl
                                .SucceedStepAsync(
                                    "Text converted to EPUB successfully",
                                    scanId
                                );
                    }
                    catch (Exception ex)
                    {
                        await
                            jobControl
                                .FailStepAsync(
                                    ex,
                                    scanId
                                );
                    }
                }
            }

            return results;
        }
    }
}