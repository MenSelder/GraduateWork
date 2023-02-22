using System;
using Registry;

namespace RegLinkInfo
{
    class UserInfo : IRegInfoGetter, IDataContainer
    {
        public RegistryHive Hive { get; }

        public string UserProfile { private set; get; }

        /*
         *  netbios имя компьютера
         *  рабочая группа 
         *  
         */

        public string WorkGroup { private set; get; }
        public string NamePC { private set; get; }

        public DateTimeOffset? LastWriteTime { get; private set; }

        public UserInfo(RegistryHive hive)
        {
            Hive = hive;            
        }

        public void RequestInfo()
        {
            UserProfile = GetCurrentUserProfile();

            NamePC = GetNamePC();
            WorkGroup = GetWorkGroup();
        }

        public void Print()
        {
            Console.WriteLine($"--- -- User Info -- ---");
            Console.WriteLine($"NamePC: {NamePC}");
            Console.WriteLine($"LastWriteTime: {LastWriteTime}");
            //Console.WriteLine($"WorkGroup: {WorkGroup}\n");
            Console.WriteLine();
        }

        private string GetCurrentUserProfile()
        {
            int profileNum;
            //RegistryKey myRegKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\Select");
            var regKey = Hive.GetKey(@"Select");

            try
            {
                object value = regKey.GetValue("Current");
                profileNum = int.Parse(value.ToString());
            }
            catch
            {
                object value = regKey.GetValue("LastKnownGood");
                profileNum = int.Parse(value.ToString());
            }

            return String.Format("{0:d3}", profileNum);
        }

        private string GetWorkGroup() // to write
        {

            return null;
        }

        private string GetNamePC()
        {    
            try
            {
                //string path = @"SYSTEM\ControlSet" + UserProfile + @"\Control\ComputerName\ComputerName";
                string path = @"ControlSet" + UserProfile + @"\Control\ComputerName\ComputerName";

                //using (RegistryKey rkLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                //using (RegistryKey rk = rkLocalMachine.OpenSubKey(path, false))
                //{
                //    object value = rk.GetValue("ComputerName");
                //    return value.ToString();
                //}
                var regKey = Hive.GetKey(path);
                LastWriteTime = regKey.LastWriteTime;
                object value = regKey.GetValue("ComputerName");
                return value.ToString();
            }
            catch { return null; }
        }
    }
}
