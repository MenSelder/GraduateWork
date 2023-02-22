using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace RegLinkInfo.RegistryData.Windows_Portable_Devices
{
    class WindowsPortableDevicesReg : BaseReg<WindowsPortableDevicesInfo>
    {
        public WindowsPortableDevicesReg(RegistryHive hive, string userProfile) : base(hive, userProfile)
        { }

        public override void RequestInfo()
        {
            if (Hive == null) return;

            string path = @"Microsoft\Windows Portable Devices\Devices";

            var regKey = Hive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            foreach (var key in regKey.SubKeys)
            {
                WindowsPortableDevicesInfo info = new WindowsPortableDevicesInfo(key.KeyName);
                string tmpPath = path + @"\" + key.KeyName;

                var tmpRegKey = Hive.GetKey(tmpPath);
                info.FriendlyName = tmpRegKey.GetValue("FriendlyName")?.ToString();

                KeyTimeStamp timeStamp = new KeyTimeStamp(
                    "Windows Portable Devices"
                    , tmpRegKey.LastWriteTime);
                info.KeyTimeStamps.Add(timeStamp);

                InfosList.Add(info);
            }
        }
    }
}
