namespace HardwareMonitor.Models
{
    public class NetworkAdapterInfo
    {
        public string Name { get; set; }
        public string MACAddress { get; set; }
        public string Status { get; set; }
        public double SpeedMbps { get; set; }
    }
}