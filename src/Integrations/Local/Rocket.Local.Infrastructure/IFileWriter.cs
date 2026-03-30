using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Local.Infrastructure
{
    public interface IFileWriter
    {
        Task WriteAsync(string fullFilePath, byte[] data, CancellationToken cancellationToken);
    }
}