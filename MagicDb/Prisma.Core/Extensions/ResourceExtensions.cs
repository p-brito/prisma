using System.Globalization;

namespace Prisma.Core.Extensions
{
    /// <summary>
    /// Defines the resource extensions class.
    /// </summary>
    internal static class ResourceExtensions
    {
        public static string Format(this string resource, string param1)
        {
            return string.Format(CultureInfo.InvariantCulture, resource, param1);
        }
    }
}
