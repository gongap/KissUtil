using System.Text;
using System.Text.RegularExpressions;
using KissUtil.Helpers;

namespace KissUtil.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 移除起始字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="start">要移除的值</param>
        public static string RemoveStart(this string value, string start)
        {
            return StringHelper.RemoveStart(value, start);
        }

        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="end">要移除的值</param>
        public static string RemoveEnd(this string value, string end)
        {
            return StringHelper.RemoveEnd(value, end);
        }

        /// <summary>
        /// 移除起始字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="start">要移除的值</param>
        public static StringBuilder RemoveStart(this StringBuilder value, string start)
        {
            return StringHelper.RemoveStart(value, start);
        }

        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="end">要移除的值</param>
        public static StringBuilder RemoveEnd(this StringBuilder value, string end)
        {
            return StringHelper.RemoveEnd(value, end);
        }

        /// <summary>
        /// 移除起始字符串
        /// </summary>
        /// <param name="writer">字符串写入器</param>
        /// <param name="start">要移除的值</param>
        public static StringWriter RemoveStart(this StringWriter writer, string start)
        {
            if (writer == null)
                return null;
            var builder = writer.GetStringBuilder();
            builder.RemoveStart(start);
            return writer;
        }

        /// <summary>
        /// 移除末尾字符串
        /// </summary>
        /// <param name="writer">字符串写入器</param>
        /// <param name="end">要移除的值</param>
        public static StringWriter RemoveEnd(this StringWriter writer, string end)
        {
            if (writer == null)
                return null;
            var builder = writer.GetStringBuilder();
            builder.RemoveEnd(end);
            return writer;
        }

        /// <summary>
        /// 确定指定的输入是否为ip.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns><c>true</c> if the specified input is ip; otherwise, <c>false</c>.</returns>
        public static bool IsIP(this string input)
        {
            return input.IsMatch(@"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\:\d{2,5}\b");
        }

        /// <summary>
        /// 确定指定的操作是否匹配.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="op">The op.</param>
        /// <returns><c>true</c> if the specified op is match; otherwise, <c>false</c>.</returns>
        public static bool IsMatch(this string str, string op)
        {
            if (str.Equals(string.Empty) || str == null)
            {
                return false;
            }

            var re = new Regex(op, RegexOptions.IgnoreCase);
            return re.IsMatch(str);
        }
    }
}
