using System.Text;
using KissUtil.Helpers;

namespace KissUtil.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtensions
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
    }
}
