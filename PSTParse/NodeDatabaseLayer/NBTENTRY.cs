using System;

namespace PSTParse.NodeDatabaseLayer
{
    /// <summary>
    /// NBTENTRY records contain information about nodes and are found in BTPAGES with cLevel equal to 0, with the ptype of ptypeNBT.
    /// </summary>
    public class NBTENTRY : BTPAGEENTRY
    {
        /// <summary>
        /// Node ID
        /// </summary>
        public ulong NID { get; set; }
        /// <summary>
        /// The Block ID of the data block for this node
        /// </summary>
        public ulong BID_Data { get; set; }
        /// <summary>
        /// The Block ID of the subnode block for this node.<br/>
        /// If this value is zero, a subnode block does not exist for this node.
        /// </summary>
        public ulong BID_SUB { get; set; }
        /// <summary>
        /// If this node represents a child of a Folder object defined in the Messaging Layer, then this value is nonzero and contains the NID of the parent Folder object's node.<br/>
        /// Otherwise, this value is zero. See section 2.2.2.7.7.4.1 for more information.
        /// <br/>This field is not interpreted by any structure defined at the NDB Layer.
        /// </summary>
        public uint NID_Parent { get; set; }
        public ulong NID_TYPE { get; set; }

        public NBTENTRY(byte[] curEntryBytes)
        {
            NID = BitConverter.ToUInt64(curEntryBytes, 0);
            BID_Data = BitConverter.ToUInt64(curEntryBytes,8);
            BID_SUB = BitConverter.ToUInt64(curEntryBytes,16);
            NID_Parent = BitConverter.ToUInt32(curEntryBytes, 24);
            NID_TYPE = NID & 0x1f;
        }
    }
}
