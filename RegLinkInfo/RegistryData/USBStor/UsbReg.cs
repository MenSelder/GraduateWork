using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;
using RegLinkInfo.RegistryData.MountedDevices;
using RegLinkInfo.RegistryData.Windows_Portable_Devices;

namespace RegLinkInfo
{
    class UsbReg : IRegInfoGetter
    {
        public RegistryHive Hive { get; }

        public List<UsbInfo> InfosList { get; private set; }

        private string userProfile;
        
        public UsbReg(RegistryHive hive, string userProfile)
        {
            Hive = hive;

            InfosList = new List<UsbInfo>();

            this.userProfile = userProfile;
        }

        public void RequestInfo()
        {
            //var regKey = Hive.GetKey(@"ControlSet" + userProfile + @"\Services\Tcpip\Parameters\Interfaces");
            //foreach (var key in regKey.SubKeys)

            /*
             * 
             * 
             */
            string path = @"ControlSet" + userProfile + @"\Enum\USBSTOR";

            var regKey = Hive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            foreach (var key in regKey.SubKeys)
            {
                path = @"ControlSet" + userProfile + @"\Enum\USBSTOR";

                string name = key.KeyName;
                UsbInfo info = new UsbInfo(name);

                path = path + @"\" + name;
                //Console.WriteLine(path);

                //using (RegistryKey rkLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                //using (RegistryKey rk = rkLocalMachine.OpenSubKey(interfacePath, false))

                /*--
                Service
                FriendlyName
                ContainerID
                AmountInstances
                */

                var rk = Hive.GetKey(path);
                {
                    //info.KeyTimeStamps.Add(new KeyTimeStamp(path, rk.LastWriteTime));
                    //Console.WriteLine(rk.KeyPath);

                    var subKeysList = rk.SubKeys;
                    string pathOrig = path;
                    path = path + @"\" + subKeysList[0].KeyName;

                    info.AmountInstances = subKeysList.Count; //qwe

                    var tmpKey = Hive.GetKey(path);
                    //Console.WriteLine(path);

                    info.FriendlyName = tmpKey.GetValue("FriendlyName")?.ToString();
                    info.Service = tmpKey.GetValue("Service")?.ToString();
                    info.ContainerID = tmpKey.GetValue("ContainerID")?.ToString();
                    //-- Instances TimeStamps
                    foreach (var k in subKeysList)
                    {
                        string tmpPath = pathOrig + @"\" + k.KeyName;
                        tmpKey = Hive.GetKey(tmpPath);
                        info.KeyTimeStamps.Add(new KeyTimeStamp(tmpPath, tmpKey.LastWriteTime));
                    }
                }

                if (info.ContainerID != null)
                {
                    path = @"ControlSet" + userProfile + @"\Enum\SWD\WPDBUSENUM";

                    string subPath = null;
                    if(Other.isFindSameGuid(path, info.ContainerID, out subPath, Hive, "ContainerID"))
                    {
                        /*
                        DiskName(FriendlyName)
                        DeviceDesc
                        Mfg - ?
                        */

                        var tmpKey = Hive.GetKey(path + @"\" + subPath);
                        info.KeyTimeStamps.Add(new KeyTimeStamp(path + @"\" + subPath, tmpKey.LastWriteTime));

                        info.DiskName = tmpKey.GetValue("FriendlyName")?.ToString();
                        info.DeviceDesc = tmpKey.GetValue("DeviceDesc")?.ToString();
                        info.Mfg = tmpKey.GetValue("Mfg")?.ToString();
                    }
                }


                InfosList.Add(info);
            }
        }

        public void SetLastConnectionTimeStamp()
        {
            string path = @"ControlSet" + userProfile 
                + @"\Control\DeviceClasses\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";

            var regKey = Hive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            string message = "Дата последнего подключения USB носителя: ";
            //InfosList.ForEach(i => Console.WriteLine(i.Guid));

            foreach (var key in regKey.SubKeys)
            {
                foreach (var usbInfo in InfosList)
                {
                    if (key.KeyName.ToUpper().Contains(usbInfo.Guid.ToUpper()))
                    {
                        //Console.WriteLine(key.KeyName.ToUpper());
                        //Console.WriteLine(usbInfo.Guid.ToUpper());
                        //Console.WriteLine("-- key : usb --");
                        usbInfo.LastConnectionTimeStamp = new KeyTimeStamp(message, key.LastWriteTime);
                    }
                }
            }
        }

        public void SetMountedDeviceInfo(MountedDevicesReg devicesInfo)
        {
            var usbDevices = devicesInfo.InfosList //test
                            .Where(i => i.DeviceData.Contains("USBSTOR"))
                            .ToList();

            // 2 foreach for every usb find divce and Set;
            foreach (var usbInfo in InfosList)
            {
                foreach (var device in usbDevices)
                {
                    var deviceName = device.DeviceData.Split('#')[1];
                    if (String.Equals(deviceName, usbInfo.Guid))
                    {
                        usbInfo.mountedDevice = device;
                        break; //?
                    }
                }
            }
        }

        public void SetPortableDeviceInfo(WindowsPortableDevicesReg portableDevices)
        {
            if (portableDevices == null) throw new NullReferenceException();
            
            foreach (var usb in InfosList)
            {
                var usbName = usb.Guid.ToLower();
                foreach (var deviceInfo in portableDevices.InfosList)
                {
                    var deviceName = deviceInfo.UsbName?.ToLower();
                    if (deviceName == null) continue;
                    if (string.Equals(deviceName, usbName))
                    {
                        usb.portableDevice = deviceInfo;
                    }
                }
            }
        }
    }
}