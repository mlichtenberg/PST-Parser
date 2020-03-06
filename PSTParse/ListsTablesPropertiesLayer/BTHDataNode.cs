using System.Collections.Generic;

namespace PSTParse.ListsTablesPropertiesLayer
{
    public class BTHDataNode
    {
        public List<BTHDataEntry> DataEntries { get; }
        public HNDataDTO Data { get; }
        public BTH Tree { get; }

        public BTHDataNode(HID hid, BTH tree)
        {
            Tree = tree;

            var bytes = tree.GetHIDBytes(hid);
            Data = bytes;
            DataEntries = new List<BTHDataEntry>();
            for (int i = 0; i < bytes.Data.Length; i += (int)(tree.Header.KeySize + tree.Header.DataSize))
                DataEntries.Add(new BTHDataEntry(bytes, i, tree));
        }
    }
}
