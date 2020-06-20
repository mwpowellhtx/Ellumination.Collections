using System.Linq;

namespace Ellumination.Collections
{
    /// <summary>
    /// Bit array extension methods provided for purposes of supporting Enumeration behavior.
    /// </summary>
    internal static class PrimitiveExtensionMethods
    {
        //TODO: consider capturing these couple of methods with an enumerated Option: Pascal case, Camel case, title Case, separate words, etc
        /// <summary>
        /// Returns the simple Camel Case abbreviation from the <paramref name="value"/> if
        /// possible. If no Camel Case letters could be identified, then simply returns the
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetCamelCaseAbbreviation(this string value)
        {
            var result = string.Join("", value.Where(char.IsUpper));
            return string.IsNullOrEmpty(result) ? value : result;
        }

        /// <summary>
        /// Returns human readable text from the string <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHumanReadableCamelCase(this string value)
        {
            var result = string.Empty;

            foreach (var v in value)
            {
                if (char.IsUpper(v))
                {
                    result += ' ';
                }

                result += v;
            }

            return result.Trim();
        }
    }
}
