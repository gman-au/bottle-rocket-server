using System.Collections.Generic;

namespace Rocket.Infrastructure.Mermaid.Extensions
{
    internal static class StringEx
    {
        public static IEnumerable<string> ToSequence(this string start)
        {
            start ??= "a";
            
            var chars = start.ToCharArray();

            while (true)
            {
                yield return new string(chars);

                // Increment the sequence
                var carry = true;
                for (var i = chars.Length - 1; i >= 0 && carry; i--)
                    if (chars[i] < 'z')
                    {
                        chars[i]++;
                        carry = false;
                    }
                    else
                    {
                        chars[i] = 'a';
                    }

                // If we still have a carry, we need an extra character
                if (!carry) continue;
                {
                    chars = new char[chars.Length + 1];
                    for (var i = 0; i < chars.Length; i++)
                        chars[i] = 'a';
                }
            }
        }
    }
}