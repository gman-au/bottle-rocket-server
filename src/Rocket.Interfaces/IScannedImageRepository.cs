using System.Threading.Tasks;
using Rocket.Domain;

namespace Rocket.Interfaces
{
    public interface IScannedImageRepository
    {
        Task<ScannedImage> SaveCaptureAsync();
    }
}