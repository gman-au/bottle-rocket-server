using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Interfaces
{
    public interface IWorkflowDetector
    {
        Task DetectAndScheduleWorkflowAsync(
            string scanId, 
            string userId,
            string modelQrCode,
            string modelQrBoundingBox,
            byte[] imageBytes,
            CancellationToken cancellationToken
        );
    }
}