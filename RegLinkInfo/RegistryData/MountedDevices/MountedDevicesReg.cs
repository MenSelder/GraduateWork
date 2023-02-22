using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace RegLinkInfo.RegistryData.MountedDevices
{
    class MountedDevicesReg : BaseReg<MountedDevicesInfo>
    {
        public MountedDevicesReg(RegistryHive hive, string userProfile) : base(hive, userProfile) { }
        public KeyTimeStamp KeyTimeStamp { get; private set; }

        public override void RequestInfo()
        {
            string path = @"MountedDevices";

            var regKey = Hive.GetKey(path);
            if (regKey == null) //key doesnt exist
                return;

            KeyTimeStamp = new KeyTimeStamp(path, regKey.LastWriteTime); //init TimeStamp
            //var currVal = string.Empty;
            //--
            try
            {
                foreach (var keyValue in regKey.Values)
                {
                    var vData = string.Empty;
                    var info = new MountedDevicesInfo(keyValue.ValueName);

                    switch (keyValue.ValueDataRaw[0])
                    {
                        case 0x7b: // {
                        case 0x5c: // \
                        case 0x5f: //_

                            vData = Encoding.Unicode.GetString(keyValue.ValueDataRaw);
                            break;

                        default:
                            //vData = CodePagesEncodingProvider.Instance.GetEncoding(1252).GetString(keyValue.ValueDataRaw);
                            info.IsSignature = true;
                            vData = BitConverter.ToString(keyValue.ValueDataRaw);
                            break;

                    }

                    //currVal = keyValue.ValueName;

                    info.DeviceName = keyValue.ValueName;
                    info.DeviceData = vData;
                    InfosList.Add(info);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            List<MountedDevicesInfo> newInfosList = new List<MountedDevicesInfo>(); //new list for swap
            ILookup<string, MountedDevicesInfo> lookUp = InfosList.ToLookup(i => i.DeviceData);
            foreach (var infosCollection in lookUp)
            {
                MountedDevicesInfo newInfo = new MountedDevicesInfo(infosCollection.Key);
                //Console.WriteLine($"infosCollection: {infosCollection.Key}");

                foreach (var info in infosCollection)
                {
                    //Console.WriteLine($"info: {info.DeviceName}");
                    newInfo.DeviceData = info.DeviceData;
                    newInfo.IsSignature = info.IsSignature;

                    if (info.DeviceName.Contains(@"\DosDevices\"))
                    {
                        newInfo.DriveLetter = info.DeviceName.Split('\\').Last();
                    }
                    else
                    {
                        newInfo.DeviceName = info.DeviceName;
                    }

                    //info.Print();
                }
                newInfosList.Add(newInfo);
                //Console.WriteLine("\n");
            }

            //infos list=new infos list 
            InfosList = newInfosList;
        } 
    }

}