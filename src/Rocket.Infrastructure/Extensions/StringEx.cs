using System;
using System.Linq;
using System.Text;

namespace Rocket.Infrastructure.Extensions
{
    public static class StringEx
    {
        public static string Obfuscate(
            this string input,
            int displayLastCharacters = 4,
            int dummyCharacters = 4,
            char obfuscationCharacter = '*'
        )
        {
            if (string.IsNullOrEmpty(input)) return null;

            var index =
                Math
                    .Min(
                        input.Length,
                        displayLastCharacters
                    );

            var substr =
                input
                    .ReverseString()[..index]
                    .ReverseString();

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < dummyCharacters; i++)
                stringBuilder
                    .Append(obfuscationCharacter);

            stringBuilder
                .Append(substr);

            return
                stringBuilder
                    .ToString();
        }

        public static string ReverseString(this string input) => new(input.Reverse().ToArray());
    }
}