using System.Net.NetworkInformation;
using System.Net.Sockets;
using KissUtil.Extensions;

namespace KissUtil.Helpers
{
    /// <summary>
    /// 网络工具.
    /// </summary>
    public class NetUtils
    {
        /// <summary>
        /// The localhost
        /// </summary>
        public const string LOCALHOST = "127.0.0.1";

        /// <summary>
        /// The anyhost
        /// </summary>
        public const string ANYHOST = "0.0.0.0";

        private const int MIN_PORT = 0;
        private const int MAX_PORT = 65535;
        private const string LOCAL_IP_PATTERN = "127(\\.\\d{1,3}){3}$";
        private const string IP_PATTERN = "\\d{1,3}(\\.\\d{1,3}){3,5}$";

        /// <summary>
        /// Determines whether [is invalid port] [the specified port].
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns><c>true</c> if [is invalid port] [the specified port]; otherwise, <c>false</c>.</returns>
        public static bool IsInvalidPort(int port)
        {
            return port <= MIN_PORT || port > MAX_PORT;
        }

        /// <summary>
        /// Determines whether [is local host] [the specified host].
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns><c>true</c> if [is local host] [the specified host]; otherwise, <c>false</c>.</returns>
        public static bool IsLocalHost(string host)
        {
            return host != null
                   && (host.IsMatch(LOCAL_IP_PATTERN)
                       || host.Equals("localhost", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether [is any host] [the specified host].
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns><c>true</c> if [is any host] [the specified host]; otherwise, <c>false</c>.</returns>
        public static bool IsAnyHost(string host)
        {
            return "0.0.0.0".Equals(host);
        }

        /// <summary>
        /// Determines whether [is valid address] [the specified address].
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if [is valid address] [the specified address]; otherwise, <c>false</c>.</returns>
        private static bool IsValidAddress(string address)
        {
            return address != null
                   && !ANYHOST.Equals(address)
                   && address.IsMatch(IP_PATTERN);
        }

        /// <summary>
        /// Determines whether [is invalid local host] [the specified host].
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns><c>true</c> if [is invalid local host] [the specified host]; otherwise, <c>false</c>.</returns>
        public static bool IsInvalidLocalHost(string host)
        {
            return host == null
                   || host.Length == 0
                   || host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                   || host.Equals("0.0.0.0")
                   || host.IsMatch(LOCAL_IP_PATTERN);
        }

        /// <summary>
        /// Gets any host address.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetAnyHostAddress()
        {
            var result = string.Empty;
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in nics)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var ipxx = adapter.GetIPProperties();
                    var ipCollection = ipxx.UnicastAddresses;
                    foreach (var ipadd in ipCollection)
                    {
                        if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            result = ipadd.Address.ToString();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the host address.
        /// </summary>
        /// <param name="hostAddress">The host address.</param>
        /// <returns>System.String.</returns>
        public static string GetHostAddress(string hostAddress)
        {
            var result = hostAddress;
            if (!IsValidAddress(hostAddress) && !IsLocalHost(hostAddress) || IsAnyHost(hostAddress))
            {
                result = GetAnyHostAddress();
            }

            return result;
        }
    }
}
