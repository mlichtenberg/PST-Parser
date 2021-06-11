using System;

namespace PSTParse.NodeDatabaseLayer
{
    /// <summary>
    /// The BREF is a record that maps a BID to its absolute file offset location.
    /// </summary>
    public class BREF
    {
        /// <summary>
        /// Block ID (BID)
        /// </summary>
        public ulong BID { get; }
        /// <summary>
        /// Byte Index (IB)
        /// </summary>
        public ulong IB { get; }
        public bool IsInternal => (BID & 0x02) > 0;

        public BREF(byte[] bref, int offset = 0)
        {
            BID = BitConverter.ToUInt64(bref, offset);
            BID = BID & 0xfffffffffffffffe;
            IB = BitConverter.ToUInt64(bref, offset + 8);

        }
    }
}
