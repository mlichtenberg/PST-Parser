using System;
using System.Linq;

namespace PSTParse.NodeDatabaseLayer
{
    /// <summary>
    /// BTENTRY records contain a key value (NID or BID) and a reference to a child BTPAGE page in the BTree.
    /// </summary>
    public class BTENTRY : BTPAGEENTRY
    {
        /// <summary>
        /// The key value associated with this BTENTRY.
        /// All the entries in the child BTPAGE referenced by BREF have key values greater than or equal to this key value.
        /// The btkey is either an NID (zero extended to 8 bytes for Unicode PSTs) or a BID, depending on the ptype of the page.
        /// </summary>
        public ulong BtKey { get; }
        /// <summary>
        /// BREF structure (section 2.2.2.4) that points to the child BTPAGE.
        /// </summary>
        public BREF BREF { get; }

        public BTENTRY(byte[] bytes)
        {
            BtKey = BitConverter.ToUInt64(bytes, 0);
            BREF = new BREF(bytes.Skip(8).Take(16).ToArray());
            /*this.BREF = new BREF_UNICODE
                            {BID_raw = BitConverter.ToUInt64(bytes, 8), ByteIndex = BitConverter.ToUInt64(bytes, 16)};*/
        }
    }
}
