using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Local.Infrastructure
{
    public class FileWriter : IFileWriter
    {
        public async Task WriteAsync(
            string fullFilePath,
            byte[] data,
            CancellationToken cancellationToken
        )
        {
            await
                File
                    .WriteAllBytesAsync(
                        fullFilePath,
                        data,
                        cancellationToken
                    );
        }
    }
}