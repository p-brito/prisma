using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicDb.Core.Extensions
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
