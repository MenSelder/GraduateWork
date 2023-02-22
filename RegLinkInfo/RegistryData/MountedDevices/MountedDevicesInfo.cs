using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo.RegistryData.MountedDevices
{
    class MountedDevicesInfo : BaseInfo
    {
        public string DeviceName { get; set; }
        public string DeviceData { get; set; }
        public string DriveLetter { get; set; }
        public bool IsSignature { get; set; }

        public new string Guid => "{" + DeviceName.Split('{').Last();

        public string MBR => DeviceData
            .Split('-')
            .Take(4)
            .Aggregate((i, j) => i + " " + j);

        public string Segment => DeviceData
            .Split('-')
            .Skip(4)
            .Reverse()
            //.Skip(3)                              // Need to Check
            .SkipWhile(s => String.Equals(s, "00")) // in literature
            //.Take(4)
            .Aggregate((i, j) => i + " " + j);

        //public bool 

        public MountedDevicesInfo(string guid) : base(guid) { IsSignature = false; }

        public override void Print()
        {
            //Console.WriteLine("#  #  #  #  #  #  #  #  #  #");
            Other.PrintValueIfNotNull("Название: ", DeviceName);
            Other.PrintValueIfNotNull("Данные: ", DeviceData);
            Other.PrintValueIfNotNull("Буква диска: ", DriveLetter);
            if (IsSignature)
            {
                PrintSignatue();
            }
            Console.WriteLine();
        }

        private void PrintSignatue()
        {
            Other.PrintValueIfNotNull("Сигнатура: ", MBR);
            Other.PrintValueIfNotNull("Сегмент: ", Segment);
        }
    }
}


//using RegistryPluginBase.Interfaces;

//namespace RegistryPlugin.MountedDevices
//{
//    public class ValuesOut : IValueOut
//    {
//        public ValuesOut(string deviceName, string deviceData)
//        {
//            DeviceName = deviceName;
//            DeviceData = deviceData;
//        }

//        public string DeviceName { get; }
//        public string DeviceData { get; }

//        public string BatchKeyPath { get; set; }
//        public string BatchValueName { get; set; }
//        public string BatchValueData1 => $"Name: {DeviceName}";
//        public string BatchValueData2 => $"Data: {DeviceData}";
//        public string BatchValueData3 => string.Empty;
//    }
//}