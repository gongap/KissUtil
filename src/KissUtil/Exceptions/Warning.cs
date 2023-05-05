using System.Text;
using KissUtil.Extensions;

namespace KissUtil.Exceptions
{
    /// <summary>
    /// 应用程序异常
    /// </summary>
    public class Warning : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Warning"/> class.
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        public Warning(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warning"/> class.
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="exception">异常</param>
        public Warning(Exception exception)
            : this(null, null, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warning"/> class.
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误码</param>
        public Warning(string message, string code)
            : this(message, code, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warning"/> class.
        /// 初始化应用程序异常
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误码</param>
        /// <param name="exception">异常</param>
        public Warning(string message, string code, Exception exception)
            : base(message ?? string.Empty, exception)
        {
            Code = code;
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 获取错误消息
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>result</returns>
        public static string GetMessage(Exception ex)
        {
            var result = new StringBuilder();
            var list = GetExceptions(ex);
            foreach (var exception in list)
            {
                AppendMessage(result, exception);
            }

            return result.ToString().RemoveEnd(Environment.NewLine);
        }

        /// <summary>
        /// 添加异常消息
        /// </summary>
        private static void AppendMessage(StringBuilder result, Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            result.AppendLine(exception.Message);
        }

        /// <summary>
        /// 获取异常列表
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns>result</returns>
        public static IList<Exception> GetExceptions(Exception ex)
        {
            var result = new List<Exception>();
            AddException(result, ex);
            return result;
        }

        /// <summary>
        /// 添加内部异常
        /// </summary>
        private static void AddException(List<Exception> result, Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            result.Add(exception);
            AddException(result, exception.InnerException);
        }

        /// <summary>
        /// 获取错误消息
        /// </summary>
        /// <returns>result</returns>
        public string GetMessage()
        {
            return GetMessage(this);
        }

        /// <summary>
        /// 获取异常列表
        /// </summary>
        /// <returns>result</returns>
        public IList<Exception> GetExceptions()
        {
            return GetExceptions(this);
        }
    }
}
