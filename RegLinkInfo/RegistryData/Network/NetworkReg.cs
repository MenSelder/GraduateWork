using System;
using System.Collections.Generic;
using System.Linq;
using Registry;

namespace RegLinkInfo
{
    class NetworkReg : IRegInfoGetter
    {
        public RegistryHive Hive { get; }

        public List<NetworkInfo> InfosList { get; private set; }

        //public DateTimeOffset? LastWriteTime { get; private set; }

        private string userProfile;

        public NetworkReg(RegistryHive hive, string userProfile)
        {
            Hive = hive;

            InfosList = new List<NetworkInfo>();

            this.userProfile = userProfile;
        }        

        public void RequestInfo() //offReg
        {
            RequestSignaturesKey("Managed");
            RequestSignaturesKey("Unmanaged");
        }

        private void RequestSignaturesKey(string subKeyName) // Unmanaged/..
        {
            string specFolderPathKey = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Signatures\" + subKeyName;
            var regKey = Hive.GetKey(specFolderPathKey);

            if (regKey == null) return; //Exit

            foreach (var keyName in regKey.SubKeys)
            {
                string str = keyName.KeyName;
                NetworkInfo info = new NetworkInfo(str);
                info.SubKey = subKeyName;

                string subPath = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Signatures\" + subKeyName + @"\" + str;
                var tmp = Hive.GetKey(subPath);
                {
                    info.LastWriteTime1 = tmp.LastWriteTime;
                    //Object val = tmp.GetValue("Description");
                    //Console.WriteLine("Название сети: {0}", val.ToString());
                    info.Description = tmp.GetValue("Description").ToString();

                    //val = tmp.GetValue("DefaultGatewayMac");
                    //Console.WriteLine("MAC: {0}", AdvancedInterfaceInfo.ByteArrayToString((byte[])val));
                    //Console.WriteLine("MAC: {0}", val.ToString());
                    info.DefaultGatewayMac = tmp.GetValue("DefaultGatewayMac").ToString();

                    //val = tmp.GetValue("DnsSuffix");
                    //Console.WriteLine("Dns: {0}", val);
                    info.DnsSuffix = tmp.GetValue("DnsSuffix").ToString();

                    //val = tmp.GetValue("FirstNetwork");
                    info.FirstNetwork = tmp.GetValue("FirstNetwork").ToString();

                    object val = tmp.GetValue("ProfileGuid");
                    info.ProfileGuid = tmp.GetValue("ProfileGuid").ToString();

                    subPath = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles\" + val.ToString();
                }
                //---
                tmp = Hive.GetKey(subPath);
                {
                    if (tmp != null)
                    {
                        info.LastWriteTime2 = tmp.LastWriteTime;

                        //Object val = tmp.GetValue("ProfileName");
                        info.ProfileName = tmp.GetValue("ProfileName").ToString();

                        object val = tmp.GetValue("DateCreated");

                        //byte[] b = (byte[])val;
                        byte[] b = val.ToString()
                            .Split('-')                               // Split into items 
                            .Select(item => Convert.ToByte(item, 16)) // Convert each item into byte
                            .ToArray();

                        DateTime date = AdvancedInterfaceInfo.GetDateFromBytes(b);
                        //Console.WriteLine("Дата создания сети: {0}", AdvancedInterfaceInfo.GetDateFromBytes((byte[])val));
                        //Console.WriteLine("Дата создания сети: {0}", date);
                        info.DateCreated = date;

                        val = tmp.GetValue("DateLastConnected");
                        b = val.ToString()
                            .Split('-')                               // Split into items 
                            .Select(item => Convert.ToByte(item, 16)) // Convert each item into byte
                            .ToArray();

                        date = AdvancedInterfaceInfo.GetDateFromBytes(b);

                        //Console.WriteLine("Дата последнего подключения: {0}", AdvancedInterfaceInfo.GetDateFromBytes((byte[])val));
                        //Console.WriteLine("Дата последнего подключения: {0}", date);
                        info.DateLastConnected = date;

                    }

                    //Console.WriteLine("");

                }
                // add
                InfosList.Add(info);
            }            
        }

        public void SetIsWireless()
        {
            string path = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Nla\Wireless";
            var regKey = Hive.GetKey(path);

            if (regKey == null) return; //Exit

            foreach (var keyName in regKey.SubKeys)
            {
                
            }
        }
    }
}


/*
string specFolderPathKey = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Signatures\Unmanaged";
            var regKey = Hive.GetKey(specFolderPathKey);

            if (regKey == null) return; //Exit

            foreach (var keyName in regKey.SubKeys)
            {
                string str = keyName.KeyName;
                NetworkInfo info = new NetworkInfo(str);

                string subPath = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Signatures\Unmanaged\" + str;
                var tmp = Hive.GetKey(subPath);
                {
                    info.LastWriteTime1 = tmp.LastWriteTime;
                    //Object val = tmp.GetValue("Description");
                    //Console.WriteLine("Название сети: {0}", val.ToString());
                    info.Description = tmp.GetValue("Description").ToString();

                    //val = tmp.GetValue("DefaultGatewayMac");
                    //Console.WriteLine("MAC: {0}", AdvancedInterfaceInfo.ByteArrayToString((byte[])val));
                    //Console.WriteLine("MAC: {0}", val.ToString());
                    info.DefaultGatewayMac = tmp.GetValue("DefaultGatewayMac").ToString();

                    //val = tmp.GetValue("DnsSuffix");
                    //Console.WriteLine("Dns: {0}", val);
                    info.DnsSuffix = tmp.GetValue("DnsSuffix").ToString();

                    //val = tmp.GetValue("FirstNetwork");
                    info.FirstNetwork = tmp.GetValue("FirstNetwork").ToString();

                    object val = tmp.GetValue("ProfileGuid");
                    info.ProfileGuid = tmp.GetValue("ProfileGuid").ToString();

                    subPath = @"Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles\" + val.ToString();
                }
                //---
                tmp = Hive.GetKey(subPath);
                {
                    if (tmp != null)
                    {
                        info.LastWriteTime2 = tmp.LastWriteTime;

                        //Object val = tmp.GetValue("ProfileName");
                        info.ProfileName = tmp.GetValue("ProfileName").ToString();

                        object val = tmp.GetValue("DateCreated");

                        //byte[] b = (byte[])val;
                        byte[] b = val.ToString()
                            .Split('-')                               // Split into items 
                            .Select(item => Convert.ToByte(item, 16)) // Convert each item into byte
                            .ToArray();

                        DateTime date = AdvancedInterfaceInfo.GetDateFromBytes(b);
                        //Console.WriteLine("Дата создания сети: {0}", AdvancedInterfaceInfo.GetDateFromBytes((byte[])val));
                        //Console.WriteLine("Дата создания сети: {0}", date);
                        info.DateCreated = date;

                        val = tmp.GetValue("DateLastConnected");
                        b = val.ToString()
                            .Split('-')                               // Split into items 
                            .Select(item => Convert.ToByte(item, 16)) // Convert each item into byte
                            .ToArray();

                        date = AdvancedInterfaceInfo.GetDateFromBytes(b);

                        //Console.WriteLine("Дата последнего подключения: {0}", AdvancedInterfaceInfo.GetDateFromBytes((byte[])val));
                        //Console.WriteLine("Дата последнего подключения: {0}", date);
                        info.DateLastConnected = date;

                    }

                    //Console.WriteLine("");

                }
                // add
                InfosList.Add(info);
            }
*/