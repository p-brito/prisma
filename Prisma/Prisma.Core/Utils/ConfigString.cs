using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Prisma.Core.Utils
{
    internal sealed class ConfigString : Dictionary<string, string>
    {
        #region Constants

        /// <summary>
        /// The pattern to parse a configuration string.
        /// </summary>
        /// <remarks>
        /// This regular expression matches multiple balanced constructs to retrieve properly-nested configuration strings
        /// within curly braces delimiters. For more information check the references below:
        /// https://weblogs.asp.net/whaggard/377025
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions.
        /// </remarks>
        private const string Pattern = @"([^;]*)=[\s]*{((?>{(?<open>)|[^{}]+|}(?<-open>))*(?(open)(?!)))}|([^;]*)=([^;]*)";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigString"/> class.
        /// </summary>
        /// <param name="value">The configuration string.</param>
        public ConfigString(string value)
            : base()
        {
            ParseValue(value, this);
            this.ConfigStr = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigString" /> class
        /// with the specified equality key comparer.
        /// </summary>
        /// <param name="value">The configuration string.</param>
        /// <param name="comparer">The equality key comparer.</param>
        public ConfigString(string value, IEqualityComparer<string> comparer)
            : base(comparer)
        {
            ParseValue(value, this);
            this.ConfigStr = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the string value that represents this configuration string.
        /// </summary>
        /// <value>The configuration string.</value>
        public string ConfigStr { get; private set; }

        #endregion

        #region Public Methods | Converters

        /// <summary>
        /// Returns a <see cref="string" /> that represents this object.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this object.
        /// </returns>
        public override string ToString()
        {
            return this.ConfigStr;
        }

        /// <summary>
        /// Returns the parsed string that represents this object.
        /// </summary>
        /// <returns>
        /// The parsed string.
        /// </returns>
        public string ToParsedString()
        {
            StringBuilder sb = new();

            foreach (string key in this.Keys)
            {
                string value;
                object obj = this[key];

                if (obj is ConfigString configString)
                {
                    value = configString.ToParsedString();
                    value = $"{{\n{value}}}";
                }
                else
                {
                    value = this[key].ToString();
                }

                string str = $"{key}: {value}";
                sb.AppendLine(str);
            }

            return sb.ToString();
        }

        #endregion

        #region Public Methods | Getters

        /// <summary>
        /// Gets the value of specified key; or throws when key is missing.
        /// </summary>
        /// <typeparam name="T">The type of expected value.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The value.
        /// </returns>
        public string GetValue(string key)
        {
            // Try to get the value of key

            if (!this.TryGetValue(key, out string value))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.RES_Exception_ConfigString_MissingKey, nameof(key)));
            }

            // Trim the white-spaces of value

            if (value is string valueString)
            {
                value = valueString.Trim();
            }

            return value;
        }


        /// <summary>
        /// Gets the value of specified key; or gets specified default value when key is missing.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value.</returns>
        public string GetValueOrDefault(string key)
        {
            // Get the value of specified key

            if (!this.TryGetValue(key, out string value))
            {
                value = string.Empty;
            }

            // Trim the white-spaces of value

            if (value is string valueString)
            {
                value = valueString.Trim();
            }

            return value;
        }

        #endregion

        #region Public Methods | Static

        /// <summary>
        /// Parses the specified value into a <see cref="ConfigString"/>.
        /// </summary>
        /// <param name="value">The value that is a representation of a configuration string.</param>
        /// <returns>The configuration string object.</returns>
        public static ConfigString Parse(string value)
        {
            return new ConfigString(value);
        }

        /// <summary>
        /// Tries to parse the specified value into a <see cref="ConfigString" />.
        /// </summary>
        /// <param name="value">The value that is a representation of a configuration string.</param>
        /// <param name="config">The configuration string object.</param>
        /// <returns>
        ///   Returns <c>true</c> if the value is a valid configuration string; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        public static bool TryParse(string value, out ConfigString config)
        {
            try
            {
                config = new ConfigString(value);
                return true;
            }
            catch
            {
                config = null;
                return false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses the specified string value, which is a representation of a configuration string,
        /// into the specified dictionary of keys and values.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="values">The dictionary of keys and values.</param>
        private static void ParseValue(string value, Dictionary<string, string> values)
        {
            // Validate parameters

            Validator.NotNull(() => values, values);

            // Null or empty value

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            // Parse the value

            MatchCollection matches = Regex.Matches(value, Pattern, RegexOptions.IgnoreCase);

            foreach (Match item in matches)
            {
                if (item.Success)
                {
                    string itemKey = item.Groups[3].Value.Trim();
                    string itemValue = item.Groups[4].Value.Trim();

                    if (string.IsNullOrEmpty(itemKey))
                    {
                        itemKey = item.Groups[1].Value.Trim();
                        itemValue = string.Empty;
                    }

                    values.Add(itemKey, itemValue);
                }
            }
        }

        #endregion

    }
}
