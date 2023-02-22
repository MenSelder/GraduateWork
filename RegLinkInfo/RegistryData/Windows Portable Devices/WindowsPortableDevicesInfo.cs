using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo.RegistryData.Windows_Portable_Devices
{
    class WindowsPortableDevicesInfo : BaseInfo
    {
        public WindowsPortableDevicesInfo(string guid) : base(guid)
        {
            Name = guid;
        }

        public string Name { get; private set; }
        public string FriendlyName { get; set; }

        public bool IsUsbStor => Name.Contains("_??_USBSTOR");
        public string UsbName => IsUsbStor ? Name.Split('#')[3] : null;
        public string InstanceID => IsUsbStor ? Name.Split('#')[4] : Name.Split('#').Last();

        public new string Guid => Name.Split('{', '}').Length > 2 
            ? "{" + Name.Split('{', '}')[1] + "}"
            : null;
                
        public override void Print()
        {
            Other.PrintValueIfNotNull("Ключ: ", Name);
            Other.PrintValueIfNotNull("GUID: ", Guid);
            Other.PrintValueIfNotNull("Упрощенное название: ", FriendlyName);
            Other.PrintValueIfNotNull("Название USB: ", UsbName);
            Other.PrintValueIfNotNull("ID экзмпляра устройства: ", InstanceID);
            
            foreach (var time in KeyTimeStamps)
            {
                time.Print();
            }
            Console.WriteLine();
        }
    }
}
