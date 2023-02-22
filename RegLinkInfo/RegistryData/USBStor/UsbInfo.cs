using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegLinkInfo.RegistryData.MountedDevices;
using RegLinkInfo.RegistryData.Windows_Portable_Devices;

namespace RegLinkInfo
{
    class UsbInfo : IDataContainer
    {
        public DateTimeOffset? LastWriteTime { get; set; }
        public List<KeyTimeStamp> KeyTimeStamps = new List<KeyTimeStamp>();

        public string Guid { private set; get; }
        /*--
        Service
        FriendlyName
        ContainerID
        AmountInstances

        DiskName(FriendlyName)
        DeviceDesc
        Mfg - ?
        */

        public string FriendlyName { get; set; }
        public string Service { get; set; }
        public string ContainerID { get; set; }
        public int AmountInstances { get; set; }
        //ControlSet001\Enum\SWD\WPDBUSENUM
        public string DiskName { get; set; }
        public string DeviceDesc { get; set; }
        public string Mfg { get; set; }
        public MountedDevicesInfo mountedDevice { get; set; }
        public WindowsPortableDevicesInfo portableDevice { get; set; }

        public KeyTimeStamp LastConnectionTimeStamp { get; set; }

        public UsbInfo(string guid)
        {
            Guid = guid;
        }

        public void Print()
        {
            //Console.WriteLine("* * * * * * * * * * * *");
            //Other.PrintValueIfNotNull("Guid: ", Guid);
            Other.PrintValueIfNotNull("Название: ", FriendlyName);
            Other.PrintValueIfNotNull("Устройство: ", Service); //?
            //Other.PrintValueIfNotNull("ContainerID: ", ContainerID);
            Other.PrintValueIfNotNull("Кол-во экземпляров записей: ", AmountInstances.ToString());

            Other.PrintValueIfNotNull("Название диска: ", DiskName);
            Other.PrintValueIfNotNull("Описание устройства: ", DeviceDesc);
            Other.PrintValueIfNotNull("Производитель: ", Mfg); //? Manufacturing

            //mountedDevice?.Print();
            Other.PrintValueIfNotNull("Буква диска: ", mountedDevice?.DriveLetter); //Disk literal

            if (!string.Equals(DiskName, portableDevice?.FriendlyName))
            {
                Other.PrintValueIfNotNull("Альтернативное название: ", portableDevice?.FriendlyName); //Additional discr Windows_Portable_Devices
            }
            //if (mountedDevice != null)
            //{
            //    mountedDevice.Print();
            //}

            //TimeSpamps
            //if (KeyTimeStamps.Count > 0)
            //{
            //    Console.WriteLine("--- Даты последних изменений ключей ---");
            //    KeyTimeStamps.ForEach(i => i.Print());
            //}
            portableDevice?.KeyTimeStamps.ForEach(i => i.Print());
            //LastConnectionTimeStamp?.Print();
            if (LastConnectionTimeStamp != null)
                Console.WriteLine("Дата последнего подключения USB носителя: " 
                    + LastConnectionTimeStamp.LastWriteTime);
            
            Console.WriteLine();
        }
    }
}
