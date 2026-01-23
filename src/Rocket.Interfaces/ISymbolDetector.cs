using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ISymbolDetector
    {
        Task<int[]> DetectSymbolMarksAsync(
            string qrCode,
            string qrCodeBoundingBox,
            byte[] imageBytes, 
            CancellationToken cancellationToken
        );
    }
}