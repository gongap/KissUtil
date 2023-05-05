using System.Collections;
using System.Text;

namespace KissUtil.Extensions
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 默认开始标记
        /// </summary>
        private const string DefaultBeginTag = "[ ";

        /// <summary>
        /// 默认结束标记
        /// </summary>
        private const string DefaultEndTag = " ]";

        /// <summary>
        /// 枚举转换成字符串
        /// </summary>
        /// <param name="reference">枚举对象</param>
        /// <param name="separator">分隔符</param>
        /// <param name="beginTag">开始标记</param>
        /// <param name="endTag">结束标记</param>
        /// <returns>转换成的字符串</returns>
        public static string ToString(this IEnumerable reference, string separator, string beginTag = DefaultBeginTag, string endTag = DefaultEndTag)
        {
            if (reference == null)
            {
                return "null";
            }

            var items = new List<object>();
            foreach (var item in reference)
            {
                items.Add(item);
            }

            return items.ToString(separator, beginTag, endTag);
        }

        /// <summary>
        /// 枚举转换成字符串
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="reference">枚举对象</param>
        /// <param name="separator">分隔符</param>
        /// <param name="beginTag">开始标记</param>
        /// <param name="endTag">结束标记</param>
        /// <returns>转换成的字符串</returns>
        public static string ToString<T>(this IEnumerable<T> reference, string separator, string beginTag = DefaultBeginTag, string endTag = DefaultEndTag)
        {
            if (reference == null)
            {
                return "null";
            }

            var info = new StringBuilder(beginTag);
            info.Append(string.Join(separator ?? string.Empty, reference));
            info.Append(endTag);
            return info.ToString();
        }

        /// <summary>
        /// 字节数组转换成 16 进制表示的字符串
        /// </summary>
        /// <param name="reference">字节数组</param>
        /// <param name="separator">分隔符</param>
        /// <param name="beginTag">开始标记</param>
        /// <param name="endTag">结束标记</param>
        /// <returns>转换后的 16 进制表示的字符串</returns>
        public static string ToHexString(this byte[] reference, string separator = null, string beginTag = DefaultBeginTag, string endTag = DefaultEndTag)
        {
            if (reference == null)
            {
                return "null";
            }

            var hex = BitConverter.ToString(reference);
            if (separator != null)
            {
                hex = hex.Replace("-", separator);
            }

            var info = new StringBuilder(beginTag);
            info.Append(hex);
            info.Append(endTag);
            return info.ToString();
        }

        /// <summary>
        /// 16 进制表示的字符串转换成字节数组
        /// </summary>
        /// <param name="reference">字节数组</param>
        /// <param name="hexString">16 进制表示的字符串</param>
        /// <param name="separator">分隔符</param>
        /// <param name="beginTag">开始标记</param>
        /// <param name="endTag">结束标记</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] FromHexString(this byte[] reference, string hexString, string separator = "-", string beginTag = DefaultBeginTag, string endTag = DefaultEndTag)
        {
            if (string.IsNullOrWhiteSpace(hexString))
            {
                return new byte[0];
            }

            hexString = hexString.Trim();
            if (!string.IsNullOrEmpty(beginTag) && hexString.StartsWith(beginTag))
            {
                hexString = hexString.Substring(beginTag.Length);
            }

            if (!string.IsNullOrEmpty(endTag) && hexString.EndsWith(endTag))
            {
                hexString = hexString.Substring(0, hexString.Length - endTag.Length);
            }

            if (!string.IsNullOrEmpty(separator))
            {
                hexString = hexString.Replace(separator, string.Empty);
            }

            var length = hexString.Length;
            var result = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                result[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return result;
        }
    }
}
