using System;

namespace PSTParse.NodeDatabaseLayer
{
    /// <summary>
    /// Leaf Block B Tree Entry.<br/>
    /// BBTENTRY records contain information about blocks and are found in BTPAGES with cLevel equal to 0, with the ptype of "ptypeBBT".<br/>
    /// These are the leaf entries of the BBT.<br/>
    /// As noted in section 2.2.2.7.7.1, these structures might not be tightly packed and the cbEnt field of the BTPAGE SHOULD be used to iterate over the entries.<br/>
    /// </summary>
    public class BBTENTRY : BTPAGEENTRY
    {
        public BREF BREF { get; }
        /// <summary>
        /// (CB)<br/>
        /// The count of bytes of the raw data contained in the block referenced by BREF excluding the block trailer and alignment padding, if any.
        /// </summary>
        public ushort BlockByteCount { get; }
        /// <summary>
        /// (CRef)<br/>
        /// Reference count indicating the count of references to this block.<br/>
        /// See section 2.2.2.7.7.3.1 regarding how reference counts work.
        /// </summary>
        public ushort RefCount { get; }
        public ulong Key => BREF.BID;
        public bool Internal => BREF.IsInternal;

        public BBTENTRY(byte[] bytes)
        {
            BREF = new BREF(bytes);
            /*this.BREF = new BREF_UNICODE
                            {BID_raw = BitConverter.ToUInt64(bytes, 0), ByteIndex = BitConverter.ToUInt64(bytes, 8)};*/
            BlockByteCount = BitConverter.ToUInt16(bytes, 16);
            RefCount = BitConverter.ToUInt16(bytes, 18);
        }
    }
}
