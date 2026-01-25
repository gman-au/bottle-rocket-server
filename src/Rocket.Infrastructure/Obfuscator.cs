using System;
using System.Linq;
using System.Text;
using Rocket.Interfaces;

namespace Rocket.Infrastructure
{
    public class Obfuscator : IObfuscator
    {
        public string Obfuscate(
            string input,
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
                ReverseString(
                    ReverseString(input)[..index]
                );

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

        private static string ReverseString(string input) => new(input.Reverse().ToArray());
    }
}