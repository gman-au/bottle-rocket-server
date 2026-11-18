using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Markdig;
using Rocket.Markdig.Infrastructure.Definition;

namespace Rocket.Markdig.Infrastructure
{
    public class EpubRenderer : IEpubRenderer
    {
        private static readonly MarkdownPipeline Pipeline =
            new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

        public async Task<byte[]> RenderToStreamAsync(
            Metadata metadata,
            IReadOnlyList<Chapter> chapters,
            string lang,
            CancellationToken cancellationToken = default
        )
        {
            using var ms = new MemoryStream();

            await
                WriteStreamAsync(
                    ms,
                    metadata,
                    chapters,
                    lang,
                    cancellationToken
                );

            return
                ms
                    .ToArray();
        }

        private async Task WriteStreamAsync(
            Stream output,
            Metadata metadata,
            IReadOnlyList<Chapter> chapters,
            string lang = "en",
            CancellationToken cancellationToken = default
        )
        {
            var encodeFunc = new Func<string, string>(WebUtility.HtmlEncode);

            using var zip =
                new ZipArchive(
                    output,
                    ZipArchiveMode.Create,
                    leaveOpen: true
                );

            // mimetype must be the first entry and stored uncompressed
            Add(
                zip,
                "mimetype",
                "application/epub+zip",
                CompressionLevel.NoCompression
            );

            Add(
                zip,
                "META-INF/container.xml",
                """
                <?xml version="1.0" encoding="utf-8"?>
                <container version="1.0" xmlns="urn:oasis:names:tc:opendocument:xmlns:container">
                  <rootfiles>
                    <rootfile full-path="OEBPS/content.opf" media-type="application/oebps-package+xml"/>
                  </rootfiles>
                </container>
                """
            );

            // Chapters
            var manifest = new StringBuilder();
            var spine = new StringBuilder();
            var toc = new StringBuilder();
            var ncx = new StringBuilder();
            for (var i = 0; i < chapters.Count; i++)
            {
                var file = $"ch{i + 1}.xhtml";
                var body = Markdown.ToHtml(
                    chapters[i].Markdown,
                    Pipeline
                );
                Add(
                    zip,
                    $"OEBPS/{file}",
                    $"""
                     <?xml version="1.0" encoding="utf-8"?>
                     <!DOCTYPE html>
                     <html xmlns="http://www.w3.org/1999/xhtml" xml:lang="{lang}">
                     <head><meta charset="utf-8"/><title>{encodeFunc(chapters[i].Title)}</title></head>
                     <body>{body}</body>
                     </html>
                     """
                );
                manifest.AppendLine($"<item id=\"ch{i + 1}\" href=\"{file}\" media-type=\"application/xhtml+xml\"/>");
                spine.AppendLine($"<itemref idref=\"ch{i + 1}\"/>");
                toc.AppendLine($"<li><a href=\"{file}\">{encodeFunc(chapters[i].Title)}</a></li>");
                ncx.AppendLine(
                    $"""
                     <navPoint id="np{i + 1}" playOrder="{i + 1}">
                       <navLabel><text>{encodeFunc(chapters[i].Title)}</text></navLabel>
                       <content src="{file}"/>
                     </navPoint>
                     """
                );
            }

            // Navigation document (required in EPUB 3)
            Add(
                zip,
                "OEBPS/nav.xhtml",
                $"""
                 <?xml version="1.0" encoding="utf-8"?>
                 <!DOCTYPE html>
                 <html xmlns="http://www.w3.org/1999/xhtml" xmlns:epub="http://www.idpf.org/2007/ops" xml:lang="{lang}">
                 <head><meta charset="utf-8"/><title>{encodeFunc(metadata.Title)}</title></head>
                 <body><nav epub:type="toc"><h1>Contents</h1><ol>{toc}</ol></nav></body>
                 </html>
                 """
            );

            // Table of contents (for EPUB 2)
            Add(
                zip,
                "OEBPS/toc.ncx",
                $"""
                 <?xml version="1.0" encoding="utf-8"?>
                 <ncx xmlns="http://www.daisy.org/z3986/2005/ncx/" version="2005-1">
                   <head>
                     <meta name="dtb:uid" content="{metadata.BookId}"/>
                     <meta name="dtb:depth" content="1"/>
                     <meta name="dtb:totalPageCount" content="0"/>
                     <meta name="dtb:maxPageNumber" content="0"/>
                   </head>
                   <docTitle><text>{encodeFunc(metadata.Title)}</text></docTitle>
                   <navMap>{ncx}</navMap>
                 </ncx>
                 """
            );

            // Package the content
            Add(
                zip,
                "OEBPS/content.opf",
                $"""
                 <?xml version="1.0" encoding="utf-8"?>
                 <package xmlns="http://www.idpf.org/2007/opf" version="3.0" unique-identifier="id">
                   <metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
                     <dc:identifier id="id">urn:uuid:{Guid.NewGuid()}</dc:identifier>
                     <dc:title>{encodeFunc(metadata.Title)}</dc:title>
                     <dc:creator>{encodeFunc(metadata.Author)}</dc:creator>
                     <dc:language>{lang}</dc:language>
                     <meta property="dcterms:modified">{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</meta>
                   </metadata>
                   <manifest>
                     <item id="nav" href="nav.xhtml" media-type="application/xhtml+xml" properties="nav"/>
                     <item id="ncx" href="toc.ncx" media-type="application/x-dtbncx+xml"/>
                     {manifest}
                   </manifest>
                   <spine>{spine}</spine>
                 </package>
                 """
            );
        }

        private static void Add(
            ZipArchive zip,
            string path,
            string content,
            CompressionLevel level = CompressionLevel.Optimal
        )
        {
            var entry =
                zip
                    .CreateEntry(
                        path,
                        level
                    );

            using var writer =
                new StreamWriter(
                    entry
                        .Open(),
                    new UTF8Encoding(false)
                );

            writer
                .Write(content);
        }
    }
}