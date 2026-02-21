using HardwareMonitor.Models;
using HardwareMonitor.Services;
using HardwareMonitor.Utils;
using HardwareMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace HardwareMonitor.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly CpuMonitor _cpuMonitor = new CpuMonitor();
        private readonly MemoryMonitor _memoryMonitor = new MemoryMonitor();
        private readonly DiskMonitor _diskMonitor = new DiskMonitor();
        private readonly GpuMonitor _gpuMonitor = new GpuMonitor();
        private readonly ExportService _exportService = new ExportService();

        private DispatcherTimer _timer;

        private CpuInfo _cpuInfo;
        public CpuInfo CpuInfo
        {
            get => _cpuInfo;
            set => SetProperty(ref _cpuInfo, value);
        }

        private MemoryInfo _memoryInfo;
        public MemoryInfo MemoryInfo
        {
            get => _memoryInfo;
            set => SetProperty(ref _memoryInfo, value);
        }

        private List<LogicalDiskInfo> _disks;
        public List<LogicalDiskInfo> Disks
        {
            get => _disks;
            set => SetProperty(ref _disks, value);
        }

        private readonly NetworkMonitor _networkMonitor = new NetworkMonitor();

        private List<NetworkAdapterInfo> _networkAdapters;
        public List<NetworkAdapterInfo> NetworkAdapters
        {
            get => _networkAdapters;
            set => SetProperty(ref _networkAdapters, value);
        }

        private GpuInfo _gpuInfo;
        public GpuInfo GpuInfo
        {
            get => _gpuInfo;
            set => SetProperty(ref _gpuInfo, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand ExportCommand { get; }

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async () => await RefreshDataAsync());
            ExportCommand = new RelayCommand(ExportData);

            StartAutoRefresh();
            Task.Run(async () => await RefreshDataAsync());
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                CpuInfo = await Task.Run(() => _cpuMonitor.GetCpuInfo());
                MemoryInfo = await Task.Run(() => _memoryMonitor.GetMemoryInfo());
                Disks = await Task.Run(() => _diskMonitor.GetLogicalDisks());
                GpuInfo = await Task.Run(() => _gpuMonitor.GetGpuInfo());
                NetworkAdapters = await Task.Run(() => _networkMonitor.GetNetworkAdapters());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void StartAutoRefresh()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += async (s, e) => await RefreshDataAsync();
            _timer.Start();
        }

        private void ExportData(object obj)
        {
            _exportService.ExportAll(CpuInfo, MemoryInfo, Disks, GpuInfo);
        }
    }
}