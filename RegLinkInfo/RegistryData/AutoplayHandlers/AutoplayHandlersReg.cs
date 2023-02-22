using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace RegLinkInfo.RegistryData.AutoplayHandlers
{
    class AutoplayHandlersReg : BaseReg<AutoplayHandlersInfo>
    {
        public AutoplayHandlersReg(RegistryHive hive, string userProfile) : base(hive, userProfile) { }

        public override void RequestInfo()
        {
            if (Hive == null)
                return; //hive not exist

            string path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers\KnownDevices";

            var regKey = Hive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            foreach (var key in regKey.SubKeys)
            {
                AutoplayHandlersInfo info = new AutoplayHandlersInfo(key.KeyName);

                var tmpKey = Hive.GetKey(path + @"\" + key.KeyName);
                info.ContainerID = tmpKey.GetValue("ContainerID")?.ToString();
                info.Label = tmpKey.GetValue("Label")?.ToString();

                info.KeyTimeStamps.Add(new KeyTimeStamp(key.KeyName, tmpKey.LastWriteTime));

                InfosList.Add(info); //Add INFO
            }
        }

        public void SetUsbInfo(UsbReg usbReg)
        {
            //usbReg.InfosList.ForEach(i => Console.WriteLine(i.ContainerID));
            //Console.WriteLine("AUTO:");
            //this.InfosList.ForEach(i => Console.WriteLine(i.ContainerID));

            foreach(var info in InfosList)
            {
                foreach (var usbInfo in usbReg.InfosList)
                {
                    if (Other.IsGuidEquals(info.ContainerID, usbInfo.ContainerID))
                    {
                        info.usbInfo = usbInfo;
                        continue;
                    }
                }
            }
        }
    }
}
