using System;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class ImageBase64Converter : IImageBase64Converter
    {
        public string Perform(byte[] imageData)
        {
            var base64 =
                Convert
                    .ToBase64String(imageData);

            return base64;
        }
    }
}