using System;
using System.Management;
using HardwareMonitor.ViewModels;

namespace HardwareMonitor.Services
{
    public class CpuMonitor
    {
        public CpuInfo GetCpuInfo()
        {
            var cpuInfo = new CpuInfo();

            try
            {
                string query = "SELECT Name, NumberOfCores, " +
                               "NumberOfLogicalProcessors, MaxClockSpeed, " +
                               "Manufacturer, Architecture " +
                               "FROM Win32_Processor";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        cpuInfo.Name = obj["Name"]?.ToString() ?? "Неизвестно";
                        cpuInfo.CoreCount = Convert.ToInt32(obj["NumberOfCores"] ?? 0);
                        cpuInfo.ThreadCount = Convert.ToInt32(obj["NumberOfLogicalProcessors"] ?? 0);
                        cpuInfo.BaseFrequency = Convert.ToInt32(obj["MaxClockSpeed"] ?? 0);
                        cpuInfo.Manufacturer = obj["Manufacturer"]?.ToString() ?? "Неизвестно";

                        int arch = Convert.ToInt32(obj["Architecture"] ?? 0);
                        cpuInfo.Architecture = arch == 9 ? "x64" : "x86";

                        break; // берём первый процессор
                    }
                }

                cpuInfo.LoadPercentage = GetCpuLoad();
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения информации о процессоре: " + ex.Message);
            }

            return cpuInfo;
        }

        private double GetCpuLoad()
        {
            try
            {
                string query = "SELECT PercentProcessorTime " +
                               "FROM Win32_PerfFormattedData_PerfOS_Processor " +
                               "WHERE Name='_Total'";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return Convert.ToDouble(obj["PercentProcessorTime"] ?? 0);
                    }
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }
    }
}