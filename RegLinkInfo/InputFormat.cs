using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RegLinkInfo
{
    class InputFormat
    {

        public static string GetFolderLink(string message = null)
        {
            if (message != null)
                Console.WriteLine(message);

            while (true)
            {
                string str = Console.ReadLine();

                if (!Directory.Exists(str))
                {
                    Console.WriteLine("Folder not Exists!\n");
                    continue;
                }

                return str;
            }
        }

        public static string GetFileLink(string message = null)
        {
            if (message != null)
                Console.WriteLine(message);

            while (true)
            {
                string str = Console.ReadLine();

                if (!File.Exists(str))
                {
                    Console.WriteLine("Input error!\nFile not Exists\n");
                    continue;
                }

                return str;
            }
        }

        public static int GetInt(string message = null)
        {
            while (true)
            {
                if (message != null)
                    Console.WriteLine(message);

                string str = Console.ReadLine();

                int value;
                if (int.TryParse(str, out value))
                {
                    return value;
                }

                Console.WriteLine("Input error!\n");

            }
        }
    }
}
