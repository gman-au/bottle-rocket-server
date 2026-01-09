using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Rocket.Web.Host.Components.Mud
{
    public static class PasswordValidation
    {
        public static IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                // empty -> won't update
                yield break;
            }

            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(
                    pw,
                    @"[A-Z]"
                ))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(
                    pw,
                    @"[a-z]"
                ))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(
                    pw,
                    @"[0-9]"
                ))
                yield return "Password must contain at least one digit";
        }
    }
}