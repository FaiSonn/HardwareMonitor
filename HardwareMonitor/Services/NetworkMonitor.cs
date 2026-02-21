using System;
using System.Collections.Generic;
using System.Management;
using HardwareMonitor.Models;

namespace HardwareMonitor.Services
{
    public class NetworkMonitor
    {
        public List<NetworkAdapterInfo> GetNetworkAdapters()
        {
            var adapters = new List<NetworkAdapterInfo>();

            string query = "SELECT Name, MACAddress, NetConnectionStatus, Speed " +
                           "FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True";

            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    var adapter = new NetworkAdapterInfo
                    {
                        Name = obj["Name"]?.ToString(),
                        MACAddress = obj["MACAddress"]?.ToString(),
                        Status = GetStatus(obj["NetConnectionStatus"]),
                        SpeedMbps = Convert.ToDouble(obj["Speed"] ?? 0) / (1024 * 1024)
                    };

                    adapters.Add(adapter);
                }
            }

            return adapters;
        }

        private string GetStatus(object status)
        {
            if (status == null) return "Unknown";

            int s = Convert.ToInt32(status);

            return s == 2 ? "Connected" : "Disconnected";
        }
    }
}