using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace RegLinkInfo.RegistryData.Printers
{
    class PrintersReg : BaseReg<PrintersInfo>
    {
        public PrintersReg(RegistryHive hive, string userProfile) 
            : base(hive, userProfile) { }

        public override void RequestInfo()
        {
            string path = @"ControlSet" + userProfile + @"\Control\Print\Printers";
            var regKey = Hive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            foreach (var key in regKey.SubKeys)
            {
                path = @"ControlSet" + userProfile + @"\Control\Print\Printers";

                string name = key.KeyName;
                PrintersInfo info = new PrintersInfo(name);

                path = path + @"\" + name;
                var rk = Hive.GetKey(path);
                {
                    info.PrinterName = rk.GetValue("Name")?.ToString();
                    info.PerUserName = rk.GetValue("PerUserName")?.ToString();
                    info.Port = rk.GetValue("Port")?.ToString();
                    info.PrinterDriver = rk.GetValue("PrinterDriver")?.ToString();

                    info.KeyTimeStamps.Add(new KeyTimeStamp(path, rk.LastWriteTime));
                }
                // add more...

                //end 
                InfosList.Add(info);
            }
        }
    }
}
