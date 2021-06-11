using System;

namespace PSTParse.NodeDatabaseLayer
{
    /// <summary>
    /// Stores metadata for a block at the end of a block
    /// </summary>
    public class BlockTrailer
    {
        /// <summary>
        /// The amount of data, in bytes, contained within the data section of the block (CB)
        /// </summary>
        public uint DataSize { get; set; }
        /// <summary>
        /// Block signature (wSig)<br/>
        /// See section 5.5 for the algorithm to calculate the block signature.
        /// </summary>
        public uint WSig { get; set; }
        /// <summary>
        /// 32-bit CRC of the CB bytes of raw data
        /// </summary>
        public uint CRC { get; set; }
        /// <summary>
        /// The Block ID of the data block
        /// </summary>
        public ulong BID_Raw { get; set; }

        public BlockTrailer(byte[] bytes, int offset)
        {
            DataSize = BitConverter.ToUInt16(bytes, offset);
            WSig = BitConverter.ToUInt16(bytes, 2 + offset);
            CRC = BitConverter.ToUInt32(bytes, 4 + offset);
            BID_Raw = BitConverter.ToUInt64(bytes, 8 + offset);
        }
    }
}
