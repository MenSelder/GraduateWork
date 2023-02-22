using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo
{
    class AdvancedInterfaceInfo : IDataContainer
    {
        /*
        InstallTimeStamp reg bin
        DriverDateData reg bin
        DriverDesc
        DriverDate
        DriverVersion
        ProviderName
        */

        public DateTime InstallTimeStamp { get; set; }
        public string NetworkAddress { get; set; }
        public string DriverDesc { get; set; }
        public string DriverDate { get; set; }
        public string DriverVersion { get; set; }
        public string ProviderName { get; set; }

        public DateTimeOffset? LastWriteTime { get; set; }

        //public void Print() //old
        //{
        //    Console.WriteLine("----- AdvancedInterfaceInfo -----");
        //    Console.WriteLine("InstallTimeStamp: " + InstallTimeStamp.ToString());
        //    Console.WriteLine("DriverDesc: " + DriverDesc);
        //    Console.WriteLine("DriverDate: " + DriverDate);
        //    Console.WriteLine("DriverVersion: " + DriverVersion);
        //    Console.WriteLine("ProviderName: " + ProviderName + "\n");
        //    if (LastWriteTime != null)
        //        Console.WriteLine("LastWriteTime: " + LastWriteTime + "\n");
        //}

        public void Print()
        {
            //Console.WriteLine();

            Other.PrintValueIfNotNull("Время установки: ", InstallTimeStamp.ToString());
            Other.PrintValueIfNotNull("MAC: ", NetworkAddress);
            Other.PrintValueIfNotNull("Описание драйвера: ", DriverDesc);
            Other.PrintValueIfNotNull("Дата драйвера: ", DriverDate);
            Other.PrintValueIfNotNull("Версия драйвера: ", DriverVersion);
            Other.PrintValueIfNotNull("Производитель: ", ProviderName);
            if (LastWriteTime != null)
                Other.PrintValueIfNotNull("Дата последнего изменения ключа: ", LastWriteTime?.ToString());

            Console.WriteLine();
        }

        static public DateTime GetDateFromBytes(byte[] binary)
        {
            if (binary == null || binary.Length != 16)
            {
                throw new ArgumentException();
            }

            return new DateTime(binary[0] + binary[1] * 256, binary[2], binary[6], binary[8], binary[10], binary[12]);
        }

        static public string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba);
        }
    }
}
