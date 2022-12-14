using Microsoft.Win32;
using System.Runtime.Versioning;

namespace WinHwMetrics
{
    [SupportedOSPlatform("windows")]
    class HwInfoSensorsReader
    {
        static string KeyName = Program.Configuration.GetSection("HwInfoRegistryKeyName").Value;
        private RegistryKey RegistryKey = Registry.CurrentUser.OpenSubKey(KeyName) ?? throw new InvalidOperationException("Registry key not found!");
        public HwInfoSensorsReader() { }

        public object ReadData()
        {
            return this.RegistryKey
                        .GetValueNames()
                        .ToList()
                        .GroupBy(e => string.Join("", e.Reverse().TakeWhile(c => char.IsDigit(c)).Reverse()))
                        .Select(e => e.ToList().ToDictionary(label => label, label => RegistryKey.GetValue(label)))
                        .ToList();
        }
    }
}
