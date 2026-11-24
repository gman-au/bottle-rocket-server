using System.Threading.Tasks;
using Rocket.Interfaces;

namespace Rocket.Integrations.Common.Extensions
{
    public static class TranscriptionControlEx
    {
        public static async Task TranscribeTextAsync(
            this ITranscriptionControl transcriptionControl,
            string scanId,
            string text
        )
        {
            await
                transcriptionControl
                    .AddTextTranscriptionCallback(
                        scanId,
                        text
                    );
        }
    }
}