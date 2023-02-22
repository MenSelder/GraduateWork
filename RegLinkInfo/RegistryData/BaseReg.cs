using System;
using System.Collections.Generic;
using Registry;

namespace RegLinkInfo.RegistryData
{
    class BaseReg<T> : IRegInfoGetter
    {
        public RegistryHive Hive { get; }
        public List<T> InfosList { get; protected set; }
        protected string userProfile;

        public BaseReg(RegistryHive hive, string userProfile)
        {
            Hive = hive;

            InfosList = new List<T>();

            this.userProfile = userProfile;
        }

        public virtual void RequestInfo() => throw new NotImplementedException();
    }
}
