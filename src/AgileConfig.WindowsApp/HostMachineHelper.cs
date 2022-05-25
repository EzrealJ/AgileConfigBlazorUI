using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.WindowsApp
{
    public class HostMachineHelper
    {
        /// <summary>
        /// 检查端口是否已被TCP监听占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool CheckPortInTcpListening(int port)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            return ipEndPoints?.Any(end => end.Port == port) ?? false;
        }

        /// <summary>
        /// 获取可用于TCP监听的端口
        /// </summary>
        /// <param name="minPort"></param>
        /// <param name="maxPort"></param>
        /// <returns></returns>
        public static ushort GetTcpListenablePort(int minPort = 0x0001, int maxPort = 0xFFFF)
        {
            var rd = new Random();
            bool listenable = false;
            int port = 0;
            while (!listenable)
            {
                port = rd.Next(minPort, maxPort);
                listenable = !CheckPortInTcpListening(port);
            }
            return (ushort)port;
        }
    }
}
