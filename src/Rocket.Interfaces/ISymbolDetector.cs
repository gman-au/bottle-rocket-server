using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface ISymbolDetector
    {
        Task<int[]> DetectSymbolMarksAsync(
            string qrCode,
            string qrCodeBoundingBox,
            string vendor,
            byte[] imageBytes, 
            CancellationToken cancellationToken
        );
    }
}