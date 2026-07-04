
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Dragon.Controller.GlobalControl.Helper
{
    public class RegexHelper
    {


        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static readonly ConcurrentDictionary<string, Regex> _regexGetCache = new();

        public static string RegexGet(string input, string pattern, string groupName = "1")
        {
            _regexGetCache.Clear();
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern))
                return string.Empty;

            // cache key = pattern + IgnoreCase
            var regex = _regexGetCache.GetOrAdd(pattern, p => new Regex(p, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));

            var match = regex.Match(input);
            if (!match.Success) return string.Empty;

            // lấy theo index
            if (int.TryParse(groupName, out int index))
            {
                return (index >= 0 && index < match.Groups.Count) ? match.Groups[index].Value : string.Empty;
            }

            // lấy theo tên
            var group = match.Groups[groupName];
            return group.Success ? group.Value : string.Empty;
        }
        public static string RegexGetABI(string input, string pattern)
        {         
            return RegexGet(input, pattern);        
        }
    }
}
