using System;

namespace RegLinkInfo
{
    class KeyTimeStamp
    {
        public string KeyName { get; }
        public DateTimeOffset? LastWriteTime { get; }

        public KeyTimeStamp(string name, DateTimeOffset? lastWriteTime)
        {
            KeyName = name;
            LastWriteTime = lastWriteTime;
        }

        public void Print()
        {
            if (LastWriteTime != null)
            {
                Console.WriteLine("Ключ: " + KeyName);
                Console.WriteLine("Дата последнего изменения: " + LastWriteTime);
            }
        }
    }
}
