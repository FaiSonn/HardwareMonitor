using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.Win32;
using HardwareMonitor.ViewModels;
using HardwareMonitor.Models;

namespace HardwareMonitor.Utils
{
    public class ExportService
    {
        public void ExportAll(CpuInfo cpu,
                              MemoryInfo memory,
                              List<LogicalDiskInfo> disks,
                              GpuInfo gpu)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON (*.json)|*.json|CSV (*.csv)|*.csv|TXT (*.txt)|*.txt";

            if (dialog.ShowDialog() != true)
                return;

            string ext = Path.GetExtension(dialog.FileName).ToLower();

            if (ext == ".json")
                ExportJson(dialog.FileName, cpu, memory, disks, gpu);
            else if (ext == ".csv")
                ExportCsv(dialog.FileName, cpu, memory, disks, gpu);
            else
                ExportTxt(dialog.FileName, cpu, memory, disks, gpu);
        }

        private void ExportTxt(string path, CpuInfo cpu,
                               MemoryInfo memory,
                               List<LogicalDiskInfo> disks,
                               GpuInfo gpu)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CPU:");
            sb.AppendLine(cpu.Name);
            sb.AppendLine(cpu.LoadPercentage + "%");

            sb.AppendLine("\nMemory:");
            sb.AppendLine(memory.TotalMemoryGB + " GB");

            sb.AppendLine("\nGPU:");
            sb.AppendLine(gpu.Name);

            sb.AppendLine("\nDisks:");
            foreach (var d in disks)
                sb.AppendLine(d.DriveLetter + " " + d.FreeSizeGB);

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private void ExportCsv(string path, CpuInfo cpu,
                               MemoryInfo memory,
                               List<LogicalDiskInfo> disks,
                               GpuInfo gpu)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Type;Name;Value");

            sb.AppendLine("CPU;Load;" + cpu.LoadPercentage);
            sb.AppendLine("Memory;TotalGB;" + memory.TotalMemoryGB);
            sb.AppendLine("GPU;Name;" + gpu.Name);

            foreach (var d in disks)
                sb.AppendLine("Disk;" + d.DriveLetter + ";" + d.FreeSizeGB);

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private void ExportJson(string path, CpuInfo cpu,
                                MemoryInfo memory,
                                List<LogicalDiskInfo> disks,
                                GpuInfo gpu)
        {
            var data = new
            {
                Cpu = cpu,
                Memory = memory,
                Gpu = gpu,
                Disks = disks,
                Date = DateTime.Now
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
}