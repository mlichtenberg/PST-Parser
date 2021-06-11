using System;

namespace PSTParse.NodeDatabaseLayer
{
    public class BID
    {
        public ulong BlockID { get; }

        public BID(byte[] bytes, int offset = 0)
        {
            BlockID = BitConverter.ToUInt64(bytes, offset) & 0xfffffffffffffffe;
        }
    }
}
