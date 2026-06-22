using System.Net.Mail;

namespace Rocket.Postmark.Infrastructure.Extensions
{
    internal static class StringEx
    {
        public static bool IsValidEmail(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return 
                MailAddress
                    .TryCreate(value, out _);
        }
    }
}