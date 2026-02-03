using System.Threading;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Rocket.QuestPdf.Infrastructure
{
    public class PdfGenerator : IPdfGenerator
    {
        public async Task<byte[]> GeneratePdfFromTextAsync(string rawText, CancellationToken cancellationToken)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var fileData =
                Document
                    .Create(
                        container =>
                        {
                            container
                                .Page(
                                    page =>
                                    {
                                        page
                                            .Size(PageSizes.A4);

                                        page
                                            .Margin(40);

                                        page
                                            .Content()
                                            .Text(
                                                text =>
                                                {
                                                    text.ParagraphSpacing(7.5F);
                                                    text.Span(rawText);
                                                }
                                            );
                                    }
                                );
                        }
                    )
                    .GeneratePdf();

            return fileData;
        }

        public async Task<byte[]> GeneratePdfFromImageAsync(byte[] imageBytes, CancellationToken cancellationToken)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var image =
                Image
                    .FromBinaryData(imageBytes);

            var fileData =
                Document
                    .Create(
                        container =>
                        {
                            container
                                .Page(
                                    page =>
                                    {
                                        page
                                            .Size(PageSizes.A4);

                                        page
                                            .Margin(20);

                                        page
                                            .Content()
                                            .Image(
                                                image
                                            );
                                    }
                                );
                        }
                    )
                    .GeneratePdf();

            return fileData;
        }
    }
}