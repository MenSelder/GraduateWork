using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Registry;

namespace RegLinkInfo
{
    public interface IRegInfoGetter
    {
        RegistryHive Hive { get; }

        void RequestInfo();
    }
}
