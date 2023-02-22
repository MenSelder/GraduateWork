using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace offlineRegBin
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"D:\Programs\hubVM\eqw\SYSTEM";
            var r = new RegistryHive(filePath);
            r.RecoverDeleted = true;
            r.ParseHive();

            var k = r.GetKey(@"ControlSet001\Control");
            Console.WriteLine(r.Root);
            //Console.WriteLine(k.InternalGuid);
            //Console.WriteLine(k.NkRecord);
            //Console.WriteLine(k.LastWriteTime);
            //foreach (var value in k.Values)
            //    Console.WriteLine(value);

            //foreach (var rUnassociatedRegistryValue in k.SubKeys)
            //    Console.WriteLine(rUnassociatedRegistryValue.KeyName);

            Console.ReadKey();
        }
    }
}
