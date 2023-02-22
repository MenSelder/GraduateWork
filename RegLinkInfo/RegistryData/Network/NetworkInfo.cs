using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo
{
    class NetworkInfo
    {
        public string Guid { private set; get; }

        /*
         * DhcpServer
         * DhcpIPAddress
         * Domain
         * IPAddress
         * SubnetMask
         * DefaultGateway
         * */

        public string Description { get; set; }
        public string DefaultGatewayMac { get; set; }
        public string DnsSuffix { get; set; }
        public string FirstNetwork { get; set; }
        public string ProfileGuid { get; set; }
        public string ProfileName { get; set; }
        public string SubKey { get; set; }
        public bool IsWireless { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastConnected { get; set; }

        public DateTimeOffset? LastWriteTime1 { get; set; }
        public DateTimeOffset? LastWriteTime2 { get; set; }

        public NetworkInfo(string guid)
        {
            Guid = guid;
        }

        public void Print()
        {
            //Console.WriteLine("* * * * * * * * * * * *");
            Other.PrintValueIfNotNull("Название сети: ", Description);
            Other.PrintValueIfNotNull("MAC Шлюза по умолчанию: ", DefaultGatewayMac);
            Other.PrintValueIfNotNull("DNS: ", DnsSuffix);
            Other.PrintValueIfNotNull("Первая сеть: ", FirstNetwork);
            Other.PrintValueIfNotNull("Тип: ", SubKey);
            //Other.PrintValueIfNotNull("Является беспроводной: ", IsWireless? "Да" : "Нет");
            Other.PrintValueIfNotNull("Имя профиля: ", ProfileName);
            Other.PrintValueIfNotNull("Дата создания сети: ", DateCreated.ToString());
            Other.PrintValueIfNotNull("Дата последнего подключения: ", DateLastConnected.ToString());

            if (LastWriteTime1 != null) Console.WriteLine("LastWriteTime1: " + LastWriteTime1);
            if (LastWriteTime2 != null) Console.WriteLine("LastWriteTime2: " + LastWriteTime2);

            Console.WriteLine();
        }

        //public void Print()
        //{            
        //    Console.WriteLine("* * * * * * * * * * * *");
        //    Console.WriteLine("Название сети: " + Description);
        //    Console.WriteLine("MAC: " + DefaultGatewayMac);
        //    Console.WriteLine("Dns: " + DnsSuffix);
        //    Console.WriteLine("Первая сеть: " + FirstNetwork);
        //    //Console.WriteLine("ProfileGuid: " + ProfileGuid);
        //    Console.WriteLine("ProfileName: " + ProfileName);
        //    Console.WriteLine("Дата создания сети: " + DateCreated);
        //    Console.WriteLine("Дата последнего подключения: " + DateCreated);
        //    if (LastWriteTime1 != null) Console.WriteLine("LastWriteTime1: " + LastWriteTime1);
        //    if (LastWriteTime2 != null) Console.WriteLine("LastWriteTime2: " + LastWriteTime2);
        //}
    }
}
