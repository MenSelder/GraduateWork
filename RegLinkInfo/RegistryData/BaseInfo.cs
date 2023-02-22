using System;
using System.Collections.Generic;

namespace RegLinkInfo.RegistryData
{
    class BaseInfo //: IDataContainer
    {
        public List<KeyTimeStamp> KeyTimeStamps = new List<KeyTimeStamp>();

        public string Name { private set; get; }

        public BaseInfo(string name)
        {
            Name = name;
        }

        public virtual void Print()
        {
            throw new NotImplementedException();
        }
    }
}
