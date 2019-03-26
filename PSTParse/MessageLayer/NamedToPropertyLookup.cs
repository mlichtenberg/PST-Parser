using System.Collections.Generic;
using PSTParse.ListsTablesPropertiesLayer;

namespace PSTParse.MessageLayer
{
    public class NamedToPropertyLookup
    {
        private const ulong NodeId = 0x61;

        public PropertyContext PC { get; set; }
        public Dictionary<ushort, NAMEID> Lookup { get; set; }

        internal byte[] GUIDs { get; set; }
        internal byte[] Entries { get; set; }
        internal byte[] String { get; set; }

        public NamedToPropertyLookup(PSTFile pst)
        {

            PC = new PropertyContext(NodeId, pst);
            GUIDs = PC.Properties[(MessageProperty)0x0002].Data;
            Entries = PC.Properties[(MessageProperty)0x0003].Data;
            String = PC.Properties[(MessageProperty)0x0004].Data;

            Lookup = new Dictionary<ushort, NAMEID>();
            for (int i = 0; i < Entries.Length; i += 8)
            {
                var cur = new NAMEID(Entries, i, this);
                Lookup.Add(cur.PropIndex, cur);
            }
        }
    }
}
