namespace PSTParse.NodeDatabaseLayer
{
    /// <summary>
    /// Blocks are the fundamental units of data storage at the NDB layer.<br/>
    /// Blocks are assigned in sizes that are multiples of 64 bytes and are aligned on 64-byte boundaries.<br/>
    /// The maximum size of any block is 8 kilobytes (8192 bytes).
    /// </summary>
    public interface IBLOCK { }
}
