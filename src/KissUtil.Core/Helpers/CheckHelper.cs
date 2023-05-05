using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace KissUtil.Helpers
{
    /// <summary>
    /// Class Check.
    /// </summary>
    [DebuggerStepThrough]
    public static class CheckHelper
    {
        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>T.</returns>
        public static T NotNull<T>(
            T value,
            [NotNull] string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <returns>T.</returns>
        public static T NotNull<T>(
            T value,
            [NotNull] string parameterName,
            string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }

            return value;
        }

        /// <summary>
        /// Nots the null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <returns>System.String.</returns>
        public static string NotNull(
            string value,
            [NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (value == null)
            {
                throw new ArgumentException($"{parameterName} can not be null!", parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }

            return value;
        }

        /// <summary>
        /// Nots the null or white space.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <returns>System.String.</returns>
        public static string NotNullOrWhiteSpace(
            string value,
            [NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }

            return value;
        }

        /// <summary>
        /// Nots the null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <returns>System.String.</returns>
        public static string NotNullOrEmpty(
            string value,
            [NotNull] string parameterName,
            int maxLength = int.MaxValue,
            int minLength = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            if (minLength > 0 && value.Length < minLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
            }

            return value;
        }

        /// <summary>
        /// Nots the null or empty.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>ICollection&lt;T&gt;.</returns>
        public static ICollection<T> NotNullOrEmpty<T>(ICollection<T> value, [NotNull] string parameterName)
        {
            if (value == null || value.Count <= 0)
            {
                throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
            }

            return value;
        }

        /// <summary>
        /// Lengthes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <returns>System.String.</returns>
        public static string Length(string value, [NotNull] string parameterName, int maxLength, int minLength = 0)
        {
            if (minLength > 0)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(parameterName + " can not be null or empty!", parameterName);
                }

                if (value.Length < minLength)
                {
                    throw new ArgumentException($"{parameterName} length must be equal to or bigger than {minLength}!", parameterName);
                }
            }

            if (value != null && value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} length must be equal to or lower than {maxLength}!", parameterName);
            }

            return value;
        } 
        
        /// <summary>
        /// 校验条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="parameterName">参数名</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">
        /// Initializes a new instance of the System.ArgumentNullException class with the
        /// name of the parameter that causes this exception
        /// </exception>
        public static void CheckCondition(Func<bool> condition, string parameterName)
        {
            if (condition.Invoke())
            {
                throw new ArgumentException(string.Format("\"{0}\"参数不能为空", parameterName));
            }
        }

        /// <summary>
        /// 校验条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="formatErrorText">格式化错误文本</param>
        /// <param name="parameters">参数数组</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">
        /// Initializes a new instance of the System.ArgumentNullException class with the
        /// name of the parameter that causes this exception
        /// </exception>
        public static void CheckCondition(Func<bool> condition, string formatErrorText, params string[] parameters)
        {
            if (condition.Invoke())
            {
                throw new ArgumentException(string.Format(formatErrorText, parameters));
            }
        }
    }
}
