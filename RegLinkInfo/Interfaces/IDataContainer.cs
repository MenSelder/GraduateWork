using System;

namespace RegLinkInfo
{
    public interface IDataContainer
    {
        DateTimeOffset? LastWriteTime { get; }
        void Print();
    }
}
