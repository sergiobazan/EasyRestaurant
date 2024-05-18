using System.Text.RegularExpressions;

namespace Domain.Expretions;

internal static class CustomExpretions
{
    private static readonly string prefixPattern = "^\\+[0-9]{2}$";
    private static readonly string numberPattern = "^9[0-9]{8}$";

    public static bool IsValidPrefix(string prefix) => Regex.IsMatch(prefix, prefixPattern);

    public static bool IsValidPhoneNumber(string phone) => Regex.IsMatch(phone, numberPattern);
}
