using System;
using System.Management;
using HardwareMonitor.ViewModels;

namespace HardwareMonitor.Services
{
    public class GpuMonitor
    {
        public GpuInfo GetGpuInfo()
        {
            var gpu = new GpuInfo();

            try
            {
                string query = "SELECT Name, AdapterRAM, DriverVersion, " +
                               "CurrentHorizontalResolution, CurrentVerticalResolution " +
                               "FROM Win32_VideoController";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        gpu.Name = obj["Name"]?.ToString() ?? "Неизвестно";
                        gpu.DriverVersion = obj["DriverVersion"]?.ToString() ?? "Неизвестно";

                        gpu.VideoMemoryGB =
                            Convert.ToDouble(obj["AdapterRAM"] ?? 0) /
                            (1024 * 1024 * 1024);

                        string width = obj["CurrentHorizontalResolution"]?.ToString();
                        string height = obj["CurrentVerticalResolution"]?.ToString();

                        gpu.Resolution = width + "x" + height;

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения информации о видеокарте: " + ex.Message);
            }

            return gpu;
        }
    }
}