using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MagicDb.Core.Utils
{
    public static class Validator
    {
        #region Public Methods

        #region NotNull

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not null, otherwise throws <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to be validated.</typeparam>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        public static void NotNull<T>(Expression<Func<T>> reference, T value)
        {
            NotNull(reference, value, Properties.Resources.RES_Exception_ArgCannotBeNull);
        }

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not null, otherwise throws <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to be validated.</typeparam>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The error message.</param>
        public static void NotNull<T>(Expression<Func<T>> reference, T value, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(GetParameterName(reference), message);
            }
        }

        #endregion

        #region NotNullOrEmpty

        #region On String

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not null or empty.
        /// Throws <see cref="ArgumentNullException"/> if the value is null.
        /// Throws <see cref="ArgumentException"/> if the value is empty.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        public static void NotNullOrEmpty(Expression<Func<string>> reference, string value)
        {
            NotNullOrEmpty(reference, value, Properties.Resources.RES_Exception_ArgCannotBeNullOrEmpty);
        }

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not null or empty.
        /// Throws <see cref="ArgumentNullException"/> if the value is null.
        /// Throws <see cref="ArgumentException"/> if the value is empty.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The error message.</param>
        public static void NotNullOrEmpty(Expression<Func<string>> reference, string value, string message)
        {
            NotNull(reference, value, message);

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException(message, GetParameterName(reference));
            }
        }

        #endregion

        #region On Array

        /// <summary>
        /// Ensures the given array <paramref name="value"/> is not null or empty. Throws <see cref="ArgumentNullException"/>
        /// in the first case, or <see cref="ArgumentException"/> in the latter.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        public static void NotNullOrEmpty(Expression<Func<object[]>> reference, object[] value)
        {
            NotNullOrEmpty(reference, value, Properties.Resources.RES_Exception_ArgCannotBeNullOrEmpty);
        }

        /// <summary>
        /// Ensures the given array <paramref name="value"/> is not null or empty. Throws <see cref="ArgumentNullException"/>
        /// in the first case, or <see cref="ArgumentException"/> in the latter.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The error message.</param>
        public static void NotNullOrEmpty(Expression<Func<object[]>> reference, object[] value, string message)
        {
            NotNull(reference, value, message);

            if (value.Length == 0)
            {
                throw new ArgumentException(message, GetParameterName(reference));
            }
        }

        #endregion

        #region On IEnumerable

        /// <summary>
        /// Ensures the given <see cref="IEnumerable"/> <paramref name="value"/> is not null or empty. Throws <see cref="ArgumentNullException"/>
        /// in the first case, or <see cref="ArgumentException"/> in the latter.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        public static void NotNullOrEmpty(Expression<Func<IEnumerable>> reference, IEnumerable value)
        {
            NotNullOrEmpty(reference, value, Properties.Resources.RES_Exception_ArgCannotBeNullOrEmpty);
        }

        /// <summary>
        /// Ensures the given <see cref="IEnumerable"/> <paramref name="value"/> is not null or empty. Throws <see cref="ArgumentNullException"/>
        /// in the first case, or <see cref="ArgumentException"/> in the latter.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The error message.</param>
        public static void NotNullOrEmpty(Expression<Func<IEnumerable>> reference, IEnumerable value, string message)
        {
            NotNull(reference, value, message);

            NotEmpty(reference, value, message);
        }

        #endregion

        #endregion

        #region NotEmpty

        #region On GUID

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not equal a <see cref="Guid.Empty"/>,
        /// otherwise throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        public static void NotEmpty(Expression<Func<Guid>> reference, Guid value)
        {
            NotEmpty(reference, value, Properties.Resources.RES_Exception_ArgCannotBeEmpty);
        }

        /// <summary>
        /// Ensures the given <paramref name="value" /> is not equal a <see cref="Guid.Empty" />,
        /// otherwise throws <see cref="ArgumentException" />.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The exception message.</param>
        public static void NotEmpty(Expression<Func<Guid>> reference, Guid value, string message)
        {
            if (value.Equals(Guid.Empty))
            {
                throw new ArgumentException(message, GetParameterName(reference));
            }
        }

        #endregion

        #region On IEnumerable

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not equal empty,
        /// otherwise throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        public static void NotEmpty(Expression<Func<IEnumerable>> reference, IEnumerable value)
        {
            NotEmpty(reference, value, Properties.Resources.RES_Exception_ArgCannotBeEmpty);
        }

        /// <summary>
        /// Ensures the given <paramref name="value"/> is not equal empty,
        /// otherwise throws <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="reference">The expression used to extract the name of the parameter.</param>
        /// <param name="value">The value to check.</param>
        /// <param name="message">The exception message.</param>
        public static void NotEmpty(Expression<Func<IEnumerable>> reference, IEnumerable value, string message)
        {
            if (value == null)
            {
                return;
            }

            if (value is string)
            {
                return;
            }

            IEnumerator enumerator = value.GetEnumerator();
            if (enumerator != null && !enumerator.MoveNext())
            {
                throw new ArgumentException(message, GetParameterName(reference));
            }
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

        private static string GetParameterName<T>(Expression<T> reference)
        {
            if (reference == null)
            {
                return null;
            }

            return ((MemberExpression)reference.Body).Member.Name;
        }

        #endregion
    }
}
