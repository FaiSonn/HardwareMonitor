using System;
using System.Collections.Generic;
using System.Management;
using HardwareMonitor.ViewModels;

namespace HardwareMonitor.Services
{
    public class DiskMonitor
    {
        public List<LogicalDiskInfo> GetLogicalDisks()
        {
            var disks = new List<LogicalDiskInfo>();

            try
            {
                string query = "SELECT DeviceID, Size, FreeSpace, FileSystem " +
                               "FROM Win32_LogicalDisk WHERE DriveType=3";

                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        var disk = new LogicalDiskInfo
                        {
                            DriveLetter = obj["DeviceID"]?.ToString(),
                            TotalSizeGB = Convert.ToDouble(obj["Size"] ?? 0) / (1024 * 1024 * 1024),
                            FreeSizeGB = Convert.ToDouble(obj["FreeSpace"] ?? 0) / (1024 * 1024 * 1024),
                            FileSystem = obj["FileSystem"]?.ToString() ?? "Неизвестно"
                        };

                        disks.Add(disk);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка получения информации о дисках: " + ex.Message);
            }

            return disks;
        }
    }
}