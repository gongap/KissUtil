namespace KissUtil.Helpers
{
    /// <summary>
    /// 公共类型.
    /// </summary>
    public static class TypeHelper
    {
        /// <summary>
        /// 对象类型
        /// </summary>
        public static Type ObjectType => typeof(object);

        /// <summary>
        /// 可转换类型
        /// </summary>
        public static Type ConvertibleType => typeof(IConvertible);

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>Type.</returns>
        public static Type GetType<T>() => GetType(typeof(T));

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>Type.</returns>
        public static Type GetType(Type type) => Nullable.GetUnderlyingType(type) ?? type;

        /// <summary>
        /// 是否为字典类型
        /// </summary>
        public static bool IsDictionary(Type type, out Type keyType, out Type valueType)
        {
            var dictionaryTypes = ReflectionHelper
                .GetImplementedGenericTypes(
                    type,
                    typeof(IDictionary<,>)
                );

            if (dictionaryTypes.Count == 1)
            {
                keyType = dictionaryTypes[0].GenericTypeArguments[0];
                valueType = dictionaryTypes[0].GenericTypeArguments[1];
                return true;
            }

            if (typeof(IDictionary<,>).IsAssignableFrom(type))
            {
                keyType = typeof(object);
                valueType = typeof(object);
                return true;
            }

            keyType = null;
            valueType = null;

            return false;
        }
    }
}
