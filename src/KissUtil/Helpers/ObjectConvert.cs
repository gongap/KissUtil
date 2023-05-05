using System.Text;
using KissUtil.Extensions;

namespace KissUtil.Helpers
{
    /// <summary>
    /// 类型转换.
    /// </summary>
    public static class ObjectConvert
    {
        /// <summary>
        /// 转换为16位整型.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>32位整型</returns>
        public static short ToShort(object input)
        {
            return ToShortOrNull(input) ?? 0;
        }

        /// <summary>
        /// 转换为16位可空整型.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>32位可空整型</returns>
        public static short? ToShortOrNull(object input)
        {
            var success = short.TryParse(input.SafeString(), out var result);
            if (success)
            {
                return result;
            }

            try
            {
                var temp = ToDoubleOrNull(input, 0);
                if (temp == null)
                {
                    return null;
                }

                return System.Convert.ToInt16(temp);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 转换为32位整型.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>32位整型</returns>
        public static int ToInt(object input)
        {
            return ToIntOrNull(input) ?? 0;
        }

        /// <summary>
        /// 转换为32位可空整型.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>32位可空整型</returns>
        public static int? ToIntOrNull(object input)
        {
            var success = int.TryParse(input.SafeString(), out var result);
            if (success)
            {
                return result;
            }

            try
            {
                var temp = ToDoubleOrNull(input, 0);
                if (temp == null)
                {
                    return null;
                }

                return System.Convert.ToInt32(temp);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 转换为64位整型.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>64位整型</returns>
        public static long ToLong(object input)
        {
            return ToLongOrNull(input) ?? 0;
        }

        /// <summary>
        /// 转换为64位可空整型.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>64位可空整型</returns>
        public static long? ToLongOrNull(object input)
        {
            var success = long.TryParse(input.SafeString(), out var result);
            if (success)
            {
                return result;
            }

            try
            {
                var temp = ToDecimalOrNull(input, 0);
                if (temp == null)
                {
                    return null;
                }

                return System.Convert.ToInt64(temp);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 转换为32位浮点型,并按指定小数位舍入.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="digits">小数位数.</param>
        /// <returns>32位浮点型</returns>
        public static float ToFloat(object input, int? digits = null)
        {
            return ToFloatOrNull(input, digits) ?? 0;
        }

        /// <summary>
        /// 转换为32位可空浮点型,并按指定小数位舍入.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="digits">小数位数.</param>
        /// <returns>32位可空浮点型</returns>
        public static float? ToFloatOrNull(object input, int? digits = null)
        {
            var success = float.TryParse(input.SafeString(), out var result);
            if (!success)
            {
                return null;
            }

            if (digits == null)
            {
                return result;
            }

            return (float)Math.Round(result, digits.Value);
        }

        /// <summary>
        /// 转换为64位浮点型,并按指定小数位舍入.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="digits">小数位数.</param>
        /// <returns>64位浮点型</returns>
        public static double ToDouble(object input, int? digits = null)
        {
            return ToDoubleOrNull(input, digits) ?? 0;
        }

        /// <summary>
        /// 转换为64位可空浮点型,并按指定小数位舍入.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="digits">小数位数.</param>
        /// <returns>64位可空浮点型</returns>
        public static double? ToDoubleOrNull(object input, int? digits = null)
        {
            var success = double.TryParse(input.SafeString(), out var result);
            if (!success)
            {
                return null;
            }

            if (digits == null)
            {
                return result;
            }

            return Math.Round(result, digits.Value);
        }

        /// <summary>
        /// 转换为128位浮点型,并按指定小数位舍入.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="digits">小数位数.</param>
        /// <returns>128位浮点型</returns>
        public static decimal ToDecimal(object input, int? digits = null)
        {
            return ToDecimalOrNull(input, digits) ?? 0;
        }

        /// <summary>
        /// 转换为128位可空浮点型,并按指定小数位舍入.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="digits">小数位数.</param>
        /// <returns>128位可空浮点型</returns>
        public static decimal? ToDecimalOrNull(object input, int? digits = null)
        {
            var success = decimal.TryParse(input.SafeString(), out var result);
            if (!success)
            {
                return null;
            }

            if (digits == null)
            {
                return result;
            }

            return Math.Round(result, digits.Value);
        }

        /// <summary>
        /// 转换为布尔值.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>布尔值</returns>
        public static bool ToBool(object input)
        {
            return ToBoolOrNull(input) ?? false;
        }

        /// <summary>
        /// 转换为可空布尔值.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>可空布尔值</returns>
        public static bool? ToBoolOrNull(object input)
        {
            var value = GetBool(input);
            if (value != null)
            {
                return value.Value;
            }

            return bool.TryParse(input.SafeString(), out var result) ? (bool?)result : null;
        }

        /// <summary>
        /// 获取布尔值.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>布尔值</returns>
        private static bool? GetBool(object input)
        {
            return input.SafeString().ToLower() switch
            {
                "0" => false,
                "否" => false,
                "不" => false,
                "no" => false,
                "fail" => false,
                "1" => true,
                "是" => true,
                "ok" => true,
                "yes" => true,
                _ => default,
            };
        }

        /// <summary>
        /// 转换为日期.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>日期</returns>
        public static DateTime ToDate(object input)
        {
            return ToDateOrNull(input) ?? DateTime.MinValue;
        }

        /// <summary>
        /// 转换为可空日期.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>可空日期</returns>
        public static DateTime? ToDateOrNull(object input)
        {
            return DateTime.TryParse(input.SafeString(), out var result) ? (DateTime?)result : null;
        }

        /// <summary>
        /// 转换为Guid.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>Guid</returns>
        public static Guid ToGuid(object input)
        {
            return ToGuidOrNull(input) ?? Guid.Empty;
        }

        /// <summary>
        /// 转换为可空Guid.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>可空Guid</returns>
        public static Guid? ToGuidOrNull(object input)
        {
            return Guid.TryParse(input.SafeString(), out var result) ? (Guid?)result : null;
        }

        /// <summary>
        /// 转换为Guid集合.
        /// </summary>
        /// <param name="input">以逗号分隔的Guid集合字符串，范例:83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A.</param>
        /// <returns>Guid集合</returns>
        public static List<Guid> ToGuidList(string input)
        {
            return ToList<Guid>(input);
        }

        /// <summary>
        /// 泛型集合转换.
        /// </summary>
        /// <typeparam name="T">目标元素类型.</typeparam>
        /// <param name="input">以逗号分隔的元素集合字符串，范例:83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A.</param>
        /// <returns>泛型集合</returns>
        public static List<T> ToList<T>(string input)
        {
            var result = new List<T>();
            if (string.IsNullOrWhiteSpace(input))
            {
                return result;
            }

            var array = input.Split(',');
            result.AddRange(from each in array where !string.IsNullOrWhiteSpace(each) select To<T>(each));
            return result;
        }

        /// <summary>
        /// 通用泛型转换.
        /// </summary>
        /// <typeparam name="T">目标类型.</typeparam>
        /// <param name="input">输入值.</param>
        /// <returns>目标</returns>
        public static T To<T>(object input)
        {
            if (input == null)
            {
                return default;
            }

            if (input is string && string.IsNullOrWhiteSpace(input.ToString()))
            {
                return default;
            }

            var type = TypeHelper.GetType<T>();
            var typeName = type.Name.ToLower();
            try
            {
                if (typeName == "string")
                {
                    return (T)(object)input.ToString();
                }

                if (typeName == "guid")
                {
                    return (T)(object)new Guid(input.ToString());
                }

                if (type.IsEnum)
                {
                    return EnumHelper.Parse<T>(input);
                }

                if (input is IConvertible)
                {
                    return (T)System.Convert.ChangeType(input, type);
                }

                return (T)input;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 转换为字节数组.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(string input)
        {
            return ToBytes(input, Encoding.UTF8);
        }

        /// <summary>
        /// 转换为字节数组.
        /// </summary>
        /// <param name="input">输入值.</param>
        /// <param name="encoding">字符编码.</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(string input, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(input) ? new byte[] { } : encoding.GetBytes(input);
        }

        /// <summary>
        /// min-max标准化,也称为离差标准化，是对原始数据的线性变换，使结果值映射到[0 - 1]之间
        /// 公式：(x - min) / (max - min)
        /// 其中max为样本数据的最大值，min为样本数据的最小值.
        /// </summary>
        /// <param name="input">输入参数.</param>
        /// <returns>归一化结果</returns>
        public static IList<(double, double)> MinMaxNormalization(this IList<(double, double)> input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new List<(double, double)>();
            var items1 = input.Select(x => x.Item1).ToList().MinMaxNormalization();
            var items2 = input.Select(x => x.Item2).ToList().MinMaxNormalization();
            for (var i = 0; i < input.Count; i++)
            {
                result.Add((items1[i], items2[2]));
            }

            return result;
        }

        /// <summary>
        /// Z-score标准化，这种方法给予原始数据的均值（mean）和标准差（standard deviation）进行数据的标准化。
        /// 经过处理的数据符合标准正态分布，即均值为0，标准差为1
        /// 公式： (x-u)/q
        /// 其中u为所有样本数据的均值，q为所有样本数据的标准差,其在取值就在（-1,1）之间.
        /// </summary>
        /// <param name="input">输入参数.</param>
        /// <returns>归一化结果</returns>
        public static IList<(double, double)> ZCore(this IList<(double, double)> input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new List<(double, double)>();
            var items1 = input.Select(x => x.Item1).ToList().MinMaxNormalization();
            var items2 = input.Select(x => x.Item2).ToList().MinMaxNormalization();
            for (var i = 0; i < input.Count; i++)
            {
                result.Add((items1[i], items2[2]));
            }

            return result;
        }

        /// <summary>
        /// min-max标准化,也称为离差标准化，是对原始数据的线性变换，使结果值映射到[0 - 1]之间
        /// 公式：(x - min) / (max - min)
        /// 其中max为样本数据的最大值，min为样本数据的最小值.
        /// </summary>
        /// <param name="input">输入参数.</param>
        /// <returns>归一化结果</returns>
        public static IList<double> MinMaxNormalization(this IList<double> input)
        {
            if (input == null)
            {
                return null;
            }

            var min = input.Min();
            var max = input.Max();
            return input.Select(x => (x - min) / (max - min)).ToList();
        }

        /// <summary>
        /// Z-score标准化，这种方法给予原始数据的均值（mean）和标准差（standard deviation）进行数据的标准化。
        /// 经过处理的数据符合标准正态分布，即均值为0，标准差为1
        /// 公式： (x-u)/q
        /// 其中u为所有样本数据的均值，q为所有样本数据的标准差,其在取值就在（-1,1）之间.
        /// </summary>
        /// <param name="input">输入参数.</param>
        /// <returns>归一化结果</returns>
        public static IList<double> ZCore(this IList<double> input)
        {
            if (input == null)
            {
                return null;
            }

            var avg = input.Average();
            var sum = input.Sum(d => Math.Pow(d - avg, 2));
            var dev = Math.Sqrt(sum / input.Count());
            return input.Select(x => (x - avg) / dev).ToList();
        }
    }
}
