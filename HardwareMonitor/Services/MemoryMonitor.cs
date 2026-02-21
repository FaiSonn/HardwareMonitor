using System;
using System.Management;
using HardwareMonitor.Models;

namespace HardwareMonitor.Services
{
    public class MemoryMonitor
    {
        public MemoryInfo GetMemoryInfo()
        {
            var memory = new MemoryInfo();

            string query = "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem";

            using (var searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    double totalKB = Convert.ToDouble(obj["TotalVisibleMemorySize"] ?? 0);
                    double freeKB = Convert.ToDouble(obj["FreePhysicalMemory"] ?? 0);

                    memory.TotalMemoryGB = Math.Round(totalKB / (1024 * 1024), 2);
                    memory.FreeMemoryGB = Math.Round(freeKB / (1024 * 1024), 2);
                    memory.UsedMemoryGB = Math.Round(memory.TotalMemoryGB - memory.FreeMemoryGB, 2);

                    if (memory.TotalMemoryGB > 0)
                        memory.UsagePercentage =
                            Math.Round((memory.UsedMemoryGB / memory.TotalMemoryGB) * 100, 2);

                    break;
                }
            }

            return memory;
        }
    }
}