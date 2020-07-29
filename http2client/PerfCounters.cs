using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;

namespace http2client
{
    public class PerfCounters
    {
        private PerformanceCounter bytesSentPerformanceCounter;
        private PerformanceCounter bytesReceivedPerformanceCounter;
        private PerformanceCounter connectionsEstablishedPerformanceCounter;

        public PerfCounters()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            NetSectionGroup netGroup = (NetSectionGroup)config.SectionGroups.Get("system.net");
            netGroup.Settings.PerformanceCounters.Enabled = true;

            config.Save(ConfigurationSaveMode.Modified); bytesSentPerformanceCounter = new PerformanceCounter();
            bytesSentPerformanceCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
            bytesSentPerformanceCounter.CounterName = "Bytes Sent";
            bytesSentPerformanceCounter.InstanceName = GetInstanceName();
            bytesSentPerformanceCounter.ReadOnly = true;

            bytesReceivedPerformanceCounter = new PerformanceCounter();
            bytesReceivedPerformanceCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
            bytesReceivedPerformanceCounter.CounterName = "Bytes Received";
            bytesReceivedPerformanceCounter.InstanceName = GetInstanceName();
            bytesReceivedPerformanceCounter.ReadOnly = true;

            connectionsEstablishedPerformanceCounter = new PerformanceCounter();
            connectionsEstablishedPerformanceCounter.CategoryName = ".NET CLR Networking 4.0.0.0";
            connectionsEstablishedPerformanceCounter.CounterName = "Connections Established";
            connectionsEstablishedPerformanceCounter.InstanceName = GetInstanceName();
            connectionsEstablishedPerformanceCounter.ReadOnly = true;
        }


        public float GetBytesSent()
        {
            float bytesSent = bytesSentPerformanceCounter.RawValue;

            return bytesSent;
        }

        public float GetBytesReceived()
        {
            float bytesReceived = bytesReceivedPerformanceCounter.RawValue;

            return bytesReceived;
        }

        public float GetConnectionsEstablished()
        {
            float bytesReceived = connectionsEstablishedPerformanceCounter.RawValue;

            return bytesReceived;
        }
        private static string GetInstanceName()
        {
            return VersioningHelper.MakeVersionSafeName("http2client.exe", ResourceScope.Machine, ResourceScope.AppDomain);
        }
    }
}
