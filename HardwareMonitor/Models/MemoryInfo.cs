namespace HardwareMonitor.Models
{
    public class MemoryInfo
    {
        public double TotalMemoryGB { get; set; }
        public double FreeMemoryGB { get; set; }
        public double UsedMemoryGB { get; set; }
        public double UsagePercentage { get; set; }
    }
}