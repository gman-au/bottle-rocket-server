using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using Rocket.Localization.Web;

namespace Rocket.Web.Host.Components.Mud
{
    internal static class PasswordValidation
    {
        public static IEnumerable<string> PasswordStrength(
            string pw,
            IStringLocalizer<PasswordResource> stringLocalizer
        )
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                // empty -> won't update
                yield break;
            }

            if (pw.Length < 8)
                yield return stringLocalizer["PasswordTooShortError"].Value;

            if (!Regex.IsMatch(
                    pw,
                    @"[A-Z]"
                ))
                yield return stringLocalizer["PasswordNoCapitalsError"].Value;
            
            if (!Regex.IsMatch(
                    pw,
                    @"[a-z]"
                ))
                yield return stringLocalizer["PasswordNoLowersError"].Value;
            
            if (!Regex.IsMatch(
                    pw,
                    @"[0-9]"
                ))
                yield return stringLocalizer["PasswordNoDigitsError"].Value;
        }
    }
}