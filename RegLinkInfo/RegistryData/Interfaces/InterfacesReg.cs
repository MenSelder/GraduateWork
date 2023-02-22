using System;
using System.Collections.Generic;
using System.Linq;
using Registry;

namespace RegLinkInfo
{
    class InterfacesReg : IRegInfoGetter
    {
        public RegistryHive Hive { get; }

        public List<InterfaceInfo> InfosList { get; private set; }

        private string userProfile;

        public InterfacesReg(RegistryHive hive, string userProfile)
        {
            Hive = hive;

            InfosList = new List<InterfaceInfo>();

            this.userProfile = userProfile;
        }

        public void RequestInfo()
        {
            var regKey = Hive.GetKey(@"ControlSet" + userProfile + @"\Services\Tcpip\Parameters\Interfaces");
            foreach(var key in regKey.SubKeys)
            {
                string name = key.KeyName;
                InterfaceInfo info = new InterfaceInfo(name);

                string interfacePath = @"ControlSet" + userProfile
                    + @"\Services\Tcpip\Parameters\Interfaces" + @"\" + name;

                //using (RegistryKey rkLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                //using (RegistryKey rk = rkLocalMachine.OpenSubKey(interfacePath, false))

                var rk = Hive.GetKey(interfacePath);
                {
                    /*
                     *  public string DhcpServer { get; set; }
                        public string DhcpIPAddress { get; set; }
                        public string Domain { get; set; }
                        public string IPAddress { get; set; }
                        public string SubnetMask { get; set; }
                        public string DefaultGateway { get; set; } 
                     */

                    info.DhcpServer = rk.GetValue("DhcpServer")?.ToString();
                    info.DhcpIPAddress = rk.GetValue("DhcpIPAddress")?.ToString();
                    info.Domain = rk.GetValue("Domain")?.ToString();
                    info.IPAddress = rk.GetValue("IPAddress")?.ToString();
                    //@interface.IPAddress = (string[])rk.GetValue("IPAddress");
                    info.SubnetMask = rk.GetValue("SubnetMask")?.ToString();
                    info.DefaultGateway = rk.GetValue("DefaultGateway")?.ToString();

                    try
                    {
                        int value = 0;

                        int.TryParse(rk.GetValue("EnableDHCP").ToString(), out value);
                        info.EnableDHCP = value;

                        int.TryParse(rk.GetValue("Lease").ToString(), out value);
                        info.Lease = value;

                        int.TryParse(rk.GetValue("LeaseObtainedTime").ToString(), out value);
                        info.LeaseObtainedTime = value;

                        int.TryParse(rk.GetValue("LeaseTerminatesTime").ToString(), out value);
                        info.LeaseTerminatesTime = value;
                    }
                    catch { }

                    info.LastWriteTime = rk.LastWriteTime;
                }

                //-----------
                string path = @"ControlSet" + userProfile + @"\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}";

                string subPath = null;
                //out
                if (isFindSameGuid(path, name, out subPath))
                {
                    //Console.WriteLine(subPath);
                    //add advanced
                    info.AdvancedInfo = RequestAdvancedInfo(path + @"\" + subPath);
                }
                //get subkey
                //link it



                //Console.WriteLine(name);
                InfosList.Add(info);
            }
        }

        private AdvancedInterfaceInfo RequestAdvancedInfo(string path)
        {
            AdvancedInterfaceInfo advInfo = new AdvancedInterfaceInfo();
            //... 
            //add params to...

            //using (RegistryKey rkLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            //using (RegistryKey rk = rkLocalMachine.OpenSubKey(path, false))

            var rk = Hive.GetKey(path);
            {
                advInfo.DriverDesc = rk.GetValue("DriverDesc")?.ToString();
                advInfo.DriverDate = rk.GetValue("DriverDate")?.ToString();
                advInfo.DriverVersion = rk.GetValue("DriverVersion")?.ToString();
                advInfo.ProviderName = rk.GetValue("ProviderName")?.ToString();
                advInfo.NetworkAddress = rk.GetValue("NetworkAddress")?.ToString();
                advInfo.LastWriteTime = rk.LastWriteTime;

                object value;
                value = rk.GetValue("InstallTimeStamp");
                if (value != null)
                {
                    //byte[] b = (byte[])value; //old ver
                    byte[] b = value.ToString()
                        .Split('-')                               // Split into items 
                        .Select(item => Convert.ToByte(item, 16)) // Convert each item into byte
                        .ToArray();

                    advInfo.InstallTimeStamp = AdvancedInterfaceInfo.GetDateFromBytes(b);
                }

                //value = rk.GetValue("DriverDateData");
                //if (value != null)
                //{
                //    byte[] b = (byte[])value;
                //    advInfo.DriverDateData = AdvancedInterfaceInfo.GetDateFromBytes(b);
                //}

                //b = (byte[])rk.GetValue("DriverDateData");
                //advInfo.DriverDateData = AdvancedInterfaceInfo.GetDateFromBytes(b);
            }

            return advInfo;
        }

        public void SetNetworkCardInfo(RegistryHive softwareHive) //Software
        {
            string path = @"Microsoft\Windows NT\CurrentVersion\NetworkCards";

            var regKey = softwareHive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            foreach (var key in regKey.SubKeys)
            {
                path = @"Microsoft\Windows NT\CurrentVersion\NetworkCards\" + key.KeyName;
                var rk = softwareHive.GetKey(path);

                string ServiceName = rk.GetValue("ServiceName")?.ToString();
                foreach (var interfaceInfo in InfosList)
                {
                    if (Other.IsGuidEquals(ServiceName, interfaceInfo.Guid))
                    {
                        //Console.WriteLine(interfaceInfo.Guid);
                        interfaceInfo.IsNetworkCard = true;
                    }
                }

            }

        }

        private bool isFindSameGuid(string regKeyPath, string guid, out string subKey)
        {
            var regKey = Hive.GetKey(regKeyPath);

            foreach (var key in regKey.SubKeys)
            {
                try
                {
                    string name = key.KeyName;
                    string tmpPath = regKeyPath + @"\" + name;

                    var tmpkey = Hive.GetKey(tmpPath);

                    if (tmpkey.GetValue("NetCfgInstanceId") != null && String.Equals(tmpkey.GetValue("NetCfgInstanceId").ToString().ToLower(), guid.ToLower()))
                    {
                        subKey = name;
                        return true;
                    }
                }
                catch { }
            }

            subKey = null;
            return false;
        }
        
        private bool HasValue(Registry.Abstractions.RegistryKey regKey, string name)
        {
            if (regKey.GetValue(name) == null) return false;

            return true;
        }
    }
}
