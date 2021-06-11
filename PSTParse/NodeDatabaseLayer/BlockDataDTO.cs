namespace PSTParse.NodeDatabaseLayer
{
    public class BlockDataDTO
    {
        public BlockDataDTO Parent { get; set; }
        public byte[] Data { get; set; }
        public ulong PstOffset { get; set; }
        public uint CRC32 { get; set; }
        public uint CRCOffset { get; set; }
        public BBTENTRY BBTEntry { get; set; }
    }
}
