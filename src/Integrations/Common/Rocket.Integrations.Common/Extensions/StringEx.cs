using System.Net.Mail;

namespace Rocket.Integrations.Common.Extensions
{
    public static class StringEx
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