using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegLinkInfo
{
    class InterfaceInfo : IDataContainer
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

        public bool IsNetworkCard { get; set; }
        public string DhcpServer { get; set; }
        public string DhcpIPAddress { get; set; }
        public string Domain { get; set; }
        public string IPAddress { get; set; }
        //public string[] IPAddress { get; set; }
        public string SubnetMask { get; set; }
        public string DefaultGateway { get; set; }
        //DHCP            
        public int EnableDHCP { get; set; }
        public int Lease { get; set; }
        public int LeaseObtainedTime { get; set; }
        public int LeaseTerminatesTime { get; set; }

        public AdvancedInterfaceInfo AdvancedInfo { set; get; }

        public DateTimeOffset? LastWriteTime { get; set; }

        public InterfaceInfo(string guid)
        {
            Guid = guid;
        }

        public void Print()
        {
            //Console.WriteLine("* * * * * * * * * * * *");
            //Other.PrintValueIfNotNull("Guid: ", Guid);
            Other.PrintValueIfNotNull("Dhcp Сервер: ", DhcpServer);
            Other.PrintValueIfNotNull("Является сетевой картой: ", IsNetworkCard? "Да" : "Нет");
            Other.PrintValueIfNotNull("Dhcp IP Адрес: ", DhcpIPAddress);
            Other.PrintValueIfNotNull("Домен: ", Domain);
            Other.PrintValueIfNotNull("IP Адрес: ", IPAddress);
            Other.PrintValueIfNotNull("Маска подсети: ", SubnetMask);
            Other.PrintValueIfNotNull("Шлюз по умолчанию: ", DefaultGateway);
            //Leases
            if (EnableDHCP == 1)
            {
                Other.PrintValueIfNotNull("Длительность лицензии DHCP: ", Lease.ToString());
                Other.PrintValueIfNotNull("Дата выдачи лицензии DHCP: ", DateTimeOffset.FromUnixTimeSeconds(LeaseObtainedTime).ToString());
                Other.PrintValueIfNotNull("Дата окончания лицензии DHCP: ", DateTimeOffset.FromUnixTimeSeconds(LeaseTerminatesTime).ToString());
            }

            if (LastWriteTime != null) Console.WriteLine("Дата последнего изменения: " + LastWriteTime);

            Console.WriteLine();
            if (AdvancedInfo != null) AdvancedInfo.Print();

            Console.WriteLine();
        }

        private string StrArrToStr (string[] strArr)
        {
            string result = String.Empty;

            if (strArr == null) return result;

            foreach (var str in strArr)
            {
                result += str + " ";
            }

            return result;
        }


    }
}
