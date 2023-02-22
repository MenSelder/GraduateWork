using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace RegLinkInfo
{
    public static class Other
    {
        public static void PrintValueIfNotNull(string name, string value)
        {
            if (value == string.Empty || value == null)
            {
                return;
            }

            Console.WriteLine(name + value);

        }

        public static string PrintValueIfNotNullToString(string name, string value)
        {
            if (value == string.Empty || value == null)
            {
                return string.Empty;
            }

            return (name + value);
        }

        public static bool isFindSameGuid(string regKeyPath, string guid, out string subKey, RegistryHive Hive, string valueName)
        {
            var regKey = Hive.GetKey(regKeyPath);

            foreach (var key in regKey.SubKeys)
            {
                try
                {
                    string name = key.KeyName;
                    string tmpPath = regKeyPath + @"\" + name;

                    var tmpkey = Hive.GetKey(tmpPath);

                    if (tmpkey.GetValue(valueName) != null && String.Equals(tmpkey.GetValue(valueName).ToString().ToLower(), guid.ToLower()))
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

        public static bool IsGuidEquals(string strA, string strB)
        {
            strA = RemoveSubStrings(strA, "{", "}");
            strB = RemoveSubStrings(strB, "{", "}");
            return string.Equals(strA.ToLower(), strB.ToLower());
        }

        public static string RemoveSubStrings(string originString, params string[] subStrings)
        {
            foreach (var subStr in subStrings)
            {
                originString.Replace(subStr, "");
            }

            return originString;
        }
    }
}
