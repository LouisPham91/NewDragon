
using System.Globalization;

namespace Dragon.Controller.GlobalControl.Helper
{
    public static class DateHelper
    {
        private static readonly string DateFormat = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// Convert string (dd/MM/yyyy HH:mm:ss) -> DateTime
        /// </summary>
        public static DateTime StringToDate(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return DateTime.MinValue;

            return DateTime.TryParseExact(
                dateStr,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime result
            ) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// Convert DateTime -> string (dd/MM/yyyy HH:mm:ss)
        /// </summary>
        public static string DateToString(DateTime date)
        {
            if (date == DateTime.MinValue)
                return string.Empty;

            return date.ToString(DateFormat, CultureInfo.InvariantCulture);
        }
    }
}
