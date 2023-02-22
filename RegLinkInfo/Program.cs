using System;
using System.IO;
using System.Linq;
using Registry;
using RegLinkInfo.RegistryData.MountedDevices;

namespace RegLinkInfo
{
    class Program
    {
        //static RegistryHive hiveSystem = null;

        static void Main(string[] args)
        {
            // -- TEST

            Database database = new Database();
            //database.Init();

            //---

            //-- D:\Programs\hubVM\eqw\SYSTEM
            //-- D:\Programs\hubVM\eqw\SOFTWARE
            //hiveSystem = GetRegistryFile("SYSTEM");

            //var userInfo = new UserInfo(hiveSystem);
            //userInfo.RequestInfo();
            //userInfo.Print();

            //-- select

            while (true)
            {
                try
                {
                    PrintHelp();
                    int mode = InputFormat.GetInt("Выберите режим работы:");
                    switch (mode)
                    {
                        case 1:
                            //PrintInterfacesInfo(userInfo);
                            //database.Interfaces.InfosList.ForEach(i => i.Print());
                            //--
                            PrintInterfaces(database);
                            break;
                        case 2:
                            //PrintNetworkInfo(userInfo);
                            //database.Network.InfosList.ForEach(i => i.Print());
                            //--
                            PrintNetwork(database);
                            break;
                        case 3:
                            //PrintUsbInfo(userInfo);
                            //database.Usb.InfosList.ForEach(i => i.Print()); // main
                            //-
                            PrintUsb(database);
                            break;
                        case 4:
                            //PrintDiskInfo(userInfo);
                            //database.MountedDevices.InfosList.ForEach(i => i.Print()); // Main
                            //                                                           //database.MountedDevices.InfosList //test
                            //                                                           //    .Where(i => i.DeviceData.Contains("USBSTOR"))
                            //                                                           //    .ToList()
                            //                                                           //    .ForEach(i => Console.WriteLine(i.DeviceData));
                            //database.MountedDevices.KeyTimeStamp.Print();
                            //--
                            PrintMountedDevices(database);
                            break;
                        case 5:
                            //database.MountedDevices.InfosList
                            //    .Where(i => i.IsSignature)
                            //    .ToList()
                            //    .ForEach(i => i.Print());
                            //--
                            PrintDisks(database);
                            break;
                        case 6: //WindowsPortableDevices
                            //database.WindowsPortableDevices.InfosList.ForEach(i => i.Print());
                            PrintWindowsPortableDevices(database);
                            break;
                        case 7: //AutoplayHandlers
                            //database.AutoplayHandlers.InfosList.ForEach(i => i.Print());

                            PrintAutoplayHandlers(database);

                            break;
                        case 8:
                            PrintPrinters(database);
                            break;
                        case 9:
                            PrintNetworkCards(database);
                            break;
                        case 0:
                            WriteAllToFile(database);
                            break;
                        default:
                            //PrintHelp();
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        //--        

        private static void PrintInterfaces(Database database)
        {
            if (database.HiveSystem == null)
            {
                Console.WriteLine("Не найден файл улья System!");
                return;
            }
            string title = "информация о сетевых интерфейсах";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.Interfaces?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        private static void PrintNetworkCards(Database database)
        {
            if (database.HiveSystem == null)
            {
                Console.WriteLine("Не найден файл улья System!");
                return;
            }
            string title = "информация о сетевых картах";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?
                .Interfaces?
                .InfosList?
                .Where(i => i.IsNetworkCard == true)
                .ToList()
                .ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        private static void PrintUsb(Database database)
        {
            if (database.HiveSystem == null)
            {
                Console.WriteLine("Не найден файл улья System!");
                return;
            }

            string title = "информация о USB носителях";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.Usb?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        private static void PrintNetwork(Database database)
        {
            if (database.HiveSoftware == null)
            {
                Console.WriteLine("Не найден файл улья Software!");
                return;
            }

            string title = "информация о сетевых подключениях";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.Network?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        private static void PrintWindowsPortableDevices(Database database)
        {
            if (database.HiveSoftware == null)
            {
                Console.WriteLine("Не найден файл улья Software!");
                return;
            }

            string title = "информация о портативный устройствах";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.WindowsPortableDevices?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        private static void PrintMountedDevices(Database database)
        {
            if (database.HiveSystem == null)
            {
                Console.WriteLine("Не найден файл улья System!");
                return;
            }

            string title = "информация о подключенных устройствах";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.MountedDevices?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });

            database.MountedDevices.KeyTimeStamp.Print();
        }

        private static void PrintDisks(Database database)
        {
            if (database.HiveSystem == null)
            {
                Console.WriteLine("Не найден файл улья System!");
                return;
            }

            string title = "информация о жестких дисках";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.MountedDevices?.InfosList
                                .Where(i => i.IsSignature)
                                .ToList()
                                .ForEach(i => 
                                {
                                    Console.Write(counter + ") ");
                                    i.Print();
                                    counter++;
                                    return;
                                });
        }

        private static void PrintAutoplayHandlers(Database database)
        {
            if (database.HiveNtUser == null)
            {
                Console.WriteLine("Не найден файл улья NtUser!");
                return;
            }

            string title = "информация о подключенных смартфонах";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.AutoplayHandlers?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        private static void PrintPrinters(Database database)
        {
            if (database.HiveNtUser == null)
            {
                Console.WriteLine("Не найден файл улья NtUser!");
                return;
            }

            string title = "информация о подключенных принтерах";
            Console.WriteLine($"--- {title.ToUpper()} ---");

            int counter = 1;
            database?.Printers?.InfosList.ForEach((i) =>
            {
                Console.Write(counter + ") ");
                i.Print();
                counter++;
                return;
            });
        }

        public static void WriteAllToFile(Database database) //??
        {
            string filePath = database.RootFolderPath + @"\outputRegInfo.txt";
            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            TextWriter baseWriter = Console.Out;

            Console.SetOut(streamWriter); //start

            PrintInterfaces(database);              //1
            PrintNetworkCards(database);            //2
            PrintNetwork(database);                 //3
            PrintUsb(database);                     //4
            PrintMountedDevices(database);          //5
            PrintDisks(database);                   //6
            PrintWindowsPortableDevices(database);  //7
            PrintAutoplayHandlers(database);        //8
            PrintPrinters(database);                //9

            //--
            Console.SetOut(baseWriter);
            streamWriter.Close();
            fileStream.Close();

            Console.WriteLine($"Данные успешно записаны! {filePath}");
        }

        private static void PrintHelp()
        {
            //
            string str =
                "1 - информация о сетевых интерфейсах\n" +
                "2 - информация о сетевых подключениях\n" +
                "3 - информация о USB носителях\n" +
                "4 - информация о подключенных устройствах\n" +
                "5 - информация о жестких дисках\n" +
                "6 - информация о портативный устройствах\n" +
                "7 - информация о подключенных смартфонах\n" +
                "8 - информация о подключенных принтерах\n" +
                "9 - информация о сетевых картах\n" +
                "0 - Записать все данные в файл\n" +
                "";

            Console.WriteLine(str);
        }
    }
}