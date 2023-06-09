namespace KissUtil.Helpers
{
    /// <summary>
    /// 时间操作
    /// </summary>
    public static class TimeHelper
    {
        /// <summary>
        /// 日期
        /// </summary>
        private static DateTime? _dateTime;

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static void SetTime(DateTime? dateTime)
        {
            _dateTime = dateTime;
        }

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static void SetTime(string dateTime)
        {
            _dateTime = ConvertHelper.ToDateOrNull(dateTime);
        }

        /// <summary>
        /// 重置时间
        /// </summary>
        public static void Reset()
        {
            _dateTime = null;
        }

        /// <summary>
        /// 获取当前日期时间
        /// </summary>
        /// <returns>DateTime.</returns>
        public static DateTime GetDateTime()
        {
            if (_dateTime == null)
                return DateTime.Now;
            return _dateTime.Value;
        }

        /// <summary>
        /// 获取当前日期,不带时间
        /// </summary>
        /// <returns>DateTime.</returns>
        public static DateTime GetDate()
        {
            return GetDateTime().Date;
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <returns>System.Int64.</returns>
        public static long GetUnixTimestamp()
        {
            return GetUnixTimestamp(DateTime.Now);
        }

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>System.Int64.</returns>
        public static long GetUnixTimestamp(DateTime time)
        {
            var start = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            var ticks = (time - start.Add(new TimeSpan(8, 0, 0))).Ticks;
            return ConvertHelper.ToLong(ticks / TimeSpan.TicksPerSecond);
        }

        /// <summary>
        /// 从Unix时间戳获取时间
        /// </summary>
        /// <param name="timestamp">Unix时间戳</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetTimeFromUnixTimestamp(long timestamp)
        {
            var start = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            var span = new TimeSpan(long.Parse(timestamp + "0000000"));
            return start.Add(span).Add(new TimeSpan(8, 0, 0));
        }

        /// <summary>
        /// 将时间标为Unix时间戳.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.Int64.</returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return System.Convert.ToInt64((dateTime - start).TotalSeconds);
        }

        /// <summary>
        /// 将时间标为Unix时间戳(毫秒).
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.Int64.</returns>
        public static long DateTimeToUnixTimestampMilliseconds(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return System.Convert.ToInt64((dateTime - start).TotalMilliseconds);
        }

        /// <summary>
        /// Unix将时间戳记为日期时间.
        /// </summary>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="time">The time.</param>
        /// <returns>DateTime.</returns>
        public static DateTime UnixTimestampToDateTime(long timestamp, DateTime? time = null)
        {
            var start = time == null
                ? new DateTime(1970, 1, 1, 0, 0, 0)
                : new DateTime(1970, 1, 1, 0, 0, 0, time.Value.Kind);
            return start.AddSeconds(timestamp);
        }
    }
}
