using System;

namespace PSTParse.NodeDatabaseLayer
{
    public enum PageType
    {
        /// <summary>
        /// Block B Tree
        /// </summary>
        BBT = 0x80,
        /// <summary>
        /// Node B Tree
        /// </summary>
        NBT = 0x81,
        FreeMap = 0x82,
        PageMap = 0x83,
        AMap = 0x84,
        FreePageMap = 0x85,
        DensityList = 0x86
    }

    /// <summary>
    /// A PAGETRAILER structure contains information about the page in which it is contained.<br/>
    /// It is present at the very end of each page in a PST file.
    /// </summary>
    public class PageTrailer
    {
        /// <summary>
        /// The type of data contained within the page.
        /// </summary>
        public PageType PageType { get; set; }
        /// <summary>
        /// The BID of the page's block.
        /// </summary>
        public ulong BID { get; set; }

        public PageTrailer(byte[] trailer)
        {
            PageType = (PageType)trailer[0];
            BID = BitConverter.ToUInt64(trailer, 8);
        }
    }
}
