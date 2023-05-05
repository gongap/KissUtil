using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace KissUtil.Helpers
{
    /// <summary>
    /// 环境操作.
    /// </summary>
    public static class EnvironmentHelper
    {
        /// <summary>
        /// 换行符.
        /// </summary>
        public static string Line => global::System.Environment.NewLine;

        /// <summary>
        /// 是否Linux操作系统.
        /// </summary>
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// 是否Windows操作系统.
        /// </summary>
        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// 是否苹果操作系统.
        /// </summary>
        public static bool IsOsx => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        /// <summary>
        /// 当前操作系统.
        /// </summary>
        public static string System => IsWindows ? "Windows" : IsLinux ? "Linux" : IsOsx ? "OSX" : string.Empty;

        /// <summary>
        /// 添加Path环境变量
        /// </summary>
        /// <param name="strPath">路径</param>
        public static void SetPathEnviroment(string strPath)
        {
            string pathList = GetSysEnviromentByName("PATH");
            bool isPathExist = false;
            if (!string.IsNullOrWhiteSpace(pathList))
            {
                // 检测是否以;结尾
                var lastValue = pathList.Substring(pathList.Length - 1, 1);
                if (lastValue != ";")
                {
                    pathList += ";";
                }

                var list = pathList.Split(";").ToList();

                if (list.Exists(a => a.Equals(strPath, StringComparison.OrdinalIgnoreCase)))
                {
                    isPathExist = true;
                }
            }

            if (!isPathExist)
            {
                SetSysEnviroment("PATH", $"{pathList}{strPath};");
            }
        }

        /// <summary>
        /// 获取系统环境变量
        /// </summary>
        /// <param name="name">变量名</param>
        /// <returns>变量值</returns>
        public static string GetSysEnviromentByName(string name)
        {
            return OpenSysEnvironment().GetValue(name).ToString();
        }

        /// <summary>
        /// 设置系统环境变量
        /// </summary>
        /// <param name="name">变量名</param>
        /// <param name="value">变量值</param>
        public static void SetSysEnviroment(string name, string value)
        {
            OpenSysEnvironment().SetValue(name, value);
        }

        /// <summary>
        /// 获取环境变量.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string GetEnvironmentVariable(string value)
        {
            var result = value;
            var param = GetParameters(result).FirstOrDefault();
            if (!string.IsNullOrEmpty(param))
            {
                var env = Environment.GetEnvironmentVariable(param);
                result = env;
                if (string.IsNullOrEmpty(env))
                {
                    var arrayData = value.Split('|');
                    result = arrayData.Length == 2 ? arrayData[1] : env;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取bool环境变量.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool GetEnvironmentVariableAsBool(string name, bool defaultValue = false)
        {
            var str = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }

            switch (str.ToLowerInvariant())
            {
                case "true":
                case "1":
                case "yes":
                    return true;
                case "false":
                case "0":
                case "no":
                    return false;
                default:
                    return defaultValue;
            }
        }

        private static List<string> GetParameters(string text)
        {
            var matchVale = new List<string>();
            var Reg = @"(?<=\${)[^\${}]*(?=})";
            var key = string.Empty;
            foreach (Match m in Regex.Matches(text, Reg))
            {
                matchVale.Add(m.Value);
            }

            return matchVale;
        }

        /// <summary>
        /// 打开系统环境变量注册表
        /// </summary>
        /// <returns>RegistryKey</returns>
        private static RegistryKey OpenSysEnvironment()
        {
            RegistryKey regLocalMachine = Registry.LocalMachine;
            RegistryKey regSystem = regLocalMachine.OpenSubKey("SYSTEM", true);
            RegistryKey regControlSet001 = regSystem.OpenSubKey("ControlSet001", true);
            RegistryKey regControl = regControlSet001.OpenSubKey("Control", true);
            RegistryKey regManager = regControl.OpenSubKey("Session Manager", true);
            RegistryKey regEnviroment = regManager.OpenSubKey("Environment", true);
            return regEnviroment;
        }
    }
}
