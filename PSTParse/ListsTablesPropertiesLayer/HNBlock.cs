using System;
using PSTParse.NodeDatabaseLayer;
using PSTParse.Utilities;

namespace PSTParse.ListsTablesPropertiesLayer
{
    /// <summary>
    /// Heap Node Block (HNBlock)
    /// </summary>
    public class HNBlock
    {
        public HNHDR Header { get; }
        public HNPAGEHDR PageHeader { get; }
        public HNBITMAPHDR BitMapPageHeader { get; }
        public HNPAGEMAP PageMap { get; }
        public ushort PageMapOffset { get; }
        private BlockDataDTO _bytes { get; }

        public HNBlock(int blockIndex, BlockDataDTO bytes)
        {
            _bytes = bytes;
            var bytesData = _bytes.Data;

            PageMapOffset = BitConverter.ToUInt16(_bytes.Data, 0);
            PageMap = new HNPAGEMAP(_bytes.Data, PageMapOffset);
            if (blockIndex == 0)
            {
                Header = new HNHDR(_bytes.Data);
            } else if (blockIndex % 128 == 8)
            {
                BitMapPageHeader = new HNBITMAPHDR(ref bytesData);
            } else
            {
                PageHeader = new HNPAGEHDR(ref bytesData);
            }
        }

        public HNDataDTO GetAllocation(HID hid)
        {
            var begOffset = PageMap.AllocationTable[(int) hid.hidIndex - 1];
            var endOffset = PageMap.AllocationTable[(int) hid.hidIndex];
            return new HNDataDTO
                       {
                           Data = _bytes.Data.RangeSubset(begOffset, endOffset - begOffset),
                           BlockOffset = begOffset,
                           Parent = _bytes
                       };
        }

        public int GetOffset()
        {
            if (Header != null)
                return 12;
            if (PageHeader != null)
                return 2;
            return 66;
        }
    }
}
