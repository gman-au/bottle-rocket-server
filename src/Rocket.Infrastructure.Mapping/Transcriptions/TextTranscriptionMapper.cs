using Rocket.Api.Contracts.Transcriptions;
using Rocket.Domain.Transcriptions;

namespace Rocket.Infrastructure.Mapping.Transcriptions
{
    public class TextTranscriptionMapper
        : TranscriptionModelMapperBase<TextTranscription, TextTranscriptionSummary>
    {
        public override TextTranscription For(
            TextTranscriptionSummary value
        )
        {
            var result =
                base
                    .For(value);
            
            result.Text = value.Text;

            return result;
        }

        public override TextTranscriptionSummary From(
            TextTranscription value
        )
        {
            var result =
                base
                    .From(value);
            
            result.Text = value.Text;

            return result;
        }
    }
}