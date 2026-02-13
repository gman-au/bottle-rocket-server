using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using QRCoder;
using Rocket.Api.Contracts.Users;
using Rocket.Domain.Enum;
using Rocket.Domain.Exceptions;
using Rocket.Interfaces;

namespace Rocket.Infrastructure.QrCode
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        private const int MaxQrCodeLengthBytes = 1663;

        public async Task<string> GenerateUserQuickAuthCodeAsync(
            UserQuickAuth value,
            CancellationToken cancellationToken
        )
        {
            if (string.IsNullOrEmpty(value?.Password))
                return null;

            var jsonString =
                JsonSerializer
                    .Serialize(value);

            if (jsonString.Length > MaxQrCodeLengthBytes)
                throw new RocketException(
                    "QR code data is too large - cannot create QR code.",
                    ApiStatusCodeEnum.ValidationError
                );

            using var qrGenerator = new QRCodeGenerator();

            using var qrCodeData =
                qrGenerator
                    .CreateQrCode(
                        jsonString,
                        QRCodeGenerator.ECCLevel.Q
                    );

            using var qrCode = new Base64QRCode(qrCodeData);

            var qrCodeImage =
                qrCode
                    .GetGraphic(20);

            return qrCodeImage;
        }
    }
}