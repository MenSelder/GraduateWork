using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Registry;
using RegLinkInfo.RegistryData.MountedDevices;
using RegLinkInfo.RegistryData.AutoplayHandlers;
using RegLinkInfo.RegistryData.Windows_Portable_Devices;
using RegLinkInfo.RegistryData.Printers;

namespace RegLinkInfo
{
    class Database
    {
        public string RootFolderPath { get; private set; }

        public RegistryHive HiveSystem { get; private set; }
        public RegistryHive HiveSoftware { get; private set; }
        public RegistryHive HiveSam { get; private set; }
        public RegistryHive HiveSecurity { get; private set; }
        public RegistryHive HiveNtUser { get; private set; }
        public RegistryHive HiveDrivers { get; private set; }
        //public RegistryHive HiveSoftware { get; private set; }
        // ... Add more hives

        //List<string> hiveNames = new List<string> { "System", "Software" }; // ?

        //Data
        public UserInfo UserInfo { get; private set; }
        public MountedDevicesReg MountedDevices { get; private set; }
        public InterfacesReg Interfaces { get; private set; }
        public UsbReg Usb { get; private set; }
        public AutoplayHandlersReg AutoplayHandlers { get; private set; }
        public PrintersReg Printers { get; private set; }
        //helpers
        public WindowsPortableDevicesReg WindowsPortableDevices { get; private set; }

        public List<IRegInfoGetter> dataSystemList = new List<IRegInfoGetter>();

        public NetworkReg Network { get; private set; }

        public Database()
        {
            // static paths
            //Init(@"C:\Users\Eredin\Desktop\RegLinkInfo\testFiles\Диплом");
            //Init(@"C:\Users\Eredin\Desktop\RegLinkInfo\testFiles");
            //Init(@"C:\Users\Eredin\Desktop\dataSnapshot"); // LAST
            //Init(@"C:\Users\Eredin\Desktop\newRegData\t2");
            Init();

            LoadData();
        }

        public void Init(string path = "")
        {
            while (true)
            {
                if (path == "")
                   path = InputFormat.GetFolderLink("Введите путь до папки с файлами пассивного реестра: ");

                int counter = 0;
                foreach (var file in Directory.GetFiles(path))
                {
                    try
                    {
                        RegistryHive hive = new RegistryHive(file);
                        
                        switch (hive.HiveType.ToString())
                        {
                            case "System":
                                HiveSystem = hive;
                                hive.ParseHive();
                                break;
                            case "Software":
                                HiveSoftware = hive;
                                hive.ParseHive();
                                break;
                            case "NtUser":
                                HiveNtUser = hive;
                                hive.ParseHive();
                                break;
                            // ...добавлять больше ульев сюда...
                            default:
                                break;
                        }

                        Console.WriteLine($"Файл {hive.HiveType.ToString()} ({file.Split('\\').Last()}) был прочитан");
                        counter++;
                    }
                    catch { }
                }

                if (counter > 0)
                {
                    RootFolderPath = path;
                    break;
                }
                Console.WriteLine("Папка не содержит файлов реестра!");
                // re Init
                path = "";
            }
        }
        
        public void LoadData() //
        {
            // Init
            if (HiveSystem != null) LoadSystemData(); //Needs SYSTEM Hive
            if (HiveSoftware != null) LoadSoftwareData(); //Needs SOFTWARE Hive
            if (HiveNtUser != null) LoadNtUserData(); //Needs HiveNtUser Hive

            //Set Linked Info
            Usb?.SetMountedDeviceInfo(MountedDevices);            
            Usb?.SetPortableDeviceInfo(WindowsPortableDevices);

            AutoplayHandlers?.SetUsbInfo(Usb); 
        }

        private void LoadSystemData()
        {
            UserInfo = new UserInfo(HiveSystem);
            UserInfo?.RequestInfo();

            MountedDevices = new MountedDevicesReg(HiveSystem, UserInfo.UserProfile);
            MountedDevices?.RequestInfo();

            Interfaces = new InterfacesReg(HiveSystem, UserInfo.UserProfile);
            Interfaces?.RequestInfo();

            Usb = new UsbReg(HiveSystem, UserInfo.UserProfile); // add Overload with MountedDevices
            Usb?.RequestInfo();
            Usb?.SetLastConnectionTimeStamp();

            Printers = new PrintersReg(HiveSystem, UserInfo.UserProfile);
            Printers?.RequestInfo();
        }

        private void LoadSoftwareData()
        {
            Network = new NetworkReg(HiveSoftware, UserInfo.UserProfile);
            Network?.RequestInfo();
            Network?.SetIsWireless();

            WindowsPortableDevices = new WindowsPortableDevicesReg(HiveSoftware, UserInfo.UserProfile);
            WindowsPortableDevices?.RequestInfo();

            Interfaces?.SetNetworkCardInfo(HiveSoftware);
        }

        private void LoadNtUserData()
        {
            AutoplayHandlers = new AutoplayHandlersReg(HiveNtUser, UserInfo.UserProfile);
            AutoplayHandlers?.RequestInfo();
        }
    }
}
