using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Markdown;
using Rocket.Domain.Jobs;
using Rocket.Integrations.Common;
using Rocket.Integrations.Common.Extensions;
using Rocket.Interfaces;

namespace Rocket.QuestPdf.Infrastructure
{
    public class PdfGenerator : IPdfGenerator
    {
        public async Task<IEnumerable<ArtifactDataElement>> GeneratePdfFromTextAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string artifactFileName,
            bool mergeFiles,
            CancellationToken cancellationToken
        )
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var results = new List<ArtifactDataElement>();
            string scanId = null;

            if (mergeFiles)
            {
                try
                {
                    var fileData =
                        Document
                            .Create(container =>
                                {
                                    foreach (var element in elements)
                                    {
                                        try
                                        {
                                            scanId = element.ScanId;
                                            container
                                                .Page(page =>
                                                    {
                                                        page
                                                            .Margin(20);
                                                        
                                                        page
                                                            .Content()
                                                            .Column(column =>
                                                                {
                                                                    column
                                                                        .Item()
                                                                        .Markdown(
                                                                            Encoding
                                                                                .Default
                                                                                .GetString(element.Artifact),
                                                                            options => { options.ParagraphSpacing = 7.5F; }
                                                                        );
                                                                }
                                                            );
                                                    }
                                                );

                                            jobControl
                                                .SucceedStepAsync(
                                                    "Text converted to merged PDF successfully",
                                                    scanId
                                                )
                                                .Wait(cancellationToken);
                                        }
                                        catch (Exception ex)
                                        {
                                            jobControl
                                                .FailStepAsync(
                                                    ex,
                                                    scanId
                                                )
                                                .Wait(cancellationToken);
                                        }
                                    }
                                }
                            )
                            .GeneratePdf();

                    results
                        .Add(
                            new ArtifactDataElement
                        {
                            Artifact = fileData,
                            FileExtension = CommonFileConstants.PdfFileExtension,
                            ScanId = null,
                            FileName = artifactFileName
                        });
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
                    scanId = element.ScanId;

                    try
                    {
                        var fileData =
                            Document
                                .Create(container =>
                                    {
                                        container
                                            .Page(page =>
                                                {
                                                    page
                                                        .Margin(20);
                                                    
                                                    page
                                                        .Content()
                                                        .Column(column =>
                                                            {
                                                                column
                                                                    .Item()
                                                                    .Markdown(
                                                                        Encoding
                                                                            .Default
                                                                            .GetString(element.Artifact),
                                                                        options => { options.ParagraphSpacing = 7.5F; }
                                                                    );
                                                            }
                                                        );
                                                }
                                            );
                                    }
                                )
                                .GeneratePdf();

                        results
                            .Add(
                                new ArtifactDataElement
                                {
                                    Artifact = fileData,
                                    FileExtension = CommonFileConstants.PdfFileExtension,
                                    ScanId = scanId,
                                    FileName = element.FileName
                                }
                            );

                        await
                            jobControl
                                .SucceedStepAsync(
                                    "Text converted to PDF successfully",
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

        public async Task<IEnumerable<ArtifactDataElement>> GeneratePdfFromImageAsync(
            IJobControl jobControl,
            IEnumerable<ArtifactDataElement> elements,
            string artifactFileName,
            bool mergeFiles,
            CancellationToken cancellationToken
        )
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var results = new List<ArtifactDataElement>();
            string scanId = null;

            if (mergeFiles)
            {
                try
                {
                    var fileData =
                        Document
                            .Create(container =>
                                {
                                    foreach (var element in elements)
                                    {
                                        try
                                        {
                                            scanId = element.ScanId;

                                            var image =
                                                Image
                                                    .FromBinaryData(element.Artifact);

                                            container
                                                .Page(page =>
                                                    {
                                                        page
                                                            .Size(PageSizes.A4);

                                                        page
                                                            .Margin(0);

                                                        page
                                                            .Content()
                                                            .Image(
                                                                image
                                                            )
                                                            .FitArea();
                                                    }
                                                );

                                            jobControl
                                                .SucceedStepAsync(
                                                    "Image converted to merged PDF successfully",
                                                    scanId
                                                )
                                                .Wait(cancellationToken);
                                        }
                                        catch (Exception ex)
                                        {
                                            jobControl
                                                .FailStepAsync(
                                                    ex,
                                                    scanId
                                                )
                                                .Wait(cancellationToken);
                                        }
                                    }
                                }
                            )
                            .GeneratePdf();

                    results
                        .Add(
                            new ArtifactDataElement
                        {
                            Artifact = fileData,
                            FileExtension = CommonFileConstants.PdfFileExtension,
                            ScanId = null,
                            FileName = artifactFileName
                        });
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
                    scanId = element.ScanId;

                    try
                    {
                        var image =
                            Image
                                .FromBinaryData(element.Artifact);

                        var fileData =
                            Document
                                .Create(container =>
                                    {
                                        container
                                            .Page(page =>
                                                {
                                                    page
                                                        .Size(PageSizes.A4);

                                                    page
                                                        .Margin(0);

                                                    page
                                                        .Content()
                                                        .Column(column =>
                                                            {
                                                                column
                                                                    .Item()
                                                                    .Image(
                                                                        image
                                                                    )
                                                                    .FitArea();
                                                            }
                                                        );
                                                }
                                            );
                                    }
                                )
                                .GeneratePdf();

                        results
                            .Add(
                                new ArtifactDataElement
                                {
                                    Artifact = fileData,
                                    FileExtension = CommonFileConstants.PdfFileExtension,
                                    ScanId = scanId,
                                    FileName = element.FileName
                                }
                            );

                        await
                            jobControl
                                .SucceedStepAsync(
                                    "Image converted to PDF successfully",
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