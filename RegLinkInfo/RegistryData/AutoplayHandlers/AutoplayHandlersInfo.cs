using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo.RegistryData.AutoplayHandlers
{
    class AutoplayHandlersInfo : BaseInfo
    {
        public string ContainerID { get; set; }
        public string Label { get; set; }

        public UsbInfo usbInfo { get; set; }

        public AutoplayHandlersInfo(string guid) : base(guid) { }

        public override void Print()
        {
            //Console.WriteLine("-------------------------------------");
            Other.PrintValueIfNotNull("Название: ", Label);
            Other.PrintValueIfNotNull("идентификатор контейнера: ", ContainerID);

            Console.WriteLine();
            usbInfo?.Print();

            KeyTimeStamps.ForEach(i => i.Print()); //Print Time
            Console.WriteLine();
        }
    }    
}
