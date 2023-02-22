using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo.RegistryData.Printers
{
    class PrintersInfo : BaseInfo
    {
        public PrintersInfo(string name) : base(name) { }

        public string PrinterName { get; set; }
        public string PerUserName { get; set; }
        public string Port { get; set; }
        public string PrinterDriver { get; set; }


        public override void Print()
        {
            Other.PrintValueIfNotNull("Название: ", PrinterName);
            Other.PrintValueIfNotNull("Название для пользователя: ", PerUserName);
            Other.PrintValueIfNotNull("Порт: ", Port);
            Other.PrintValueIfNotNull("Драйвер: ", PrinterDriver);

            KeyTimeStamps.ForEach(i => i.Print()); //Print Time

            Console.WriteLine();
        }
    }
}
