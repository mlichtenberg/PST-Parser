using System;
using System.Collections.Generic;
using PSTParse.NodeDatabaseLayer;

namespace PSTParse.ListsTablesPropertiesLayer
{
    public class TableContext
    {
        public TCINFOHEADER TCHeader { get; }
        public HN HeapNode { get; }
        public NodeDataDTO NodeData { get; }
        public BTH RowIndexBTH { get; }
        public Dictionary<uint, uint> ReverseRowIndex { get; }
        public TCRowMatrix RowMatrix { get; }

        public TableContext(ulong nid, PSTFile pst)
        {
            NodeData = BlockBO.GetNodeData(nid, pst);

            HeapNode = new HN(NodeData);

            var tcinfoHID = HeapNode.HeapNodes[0].Header.UserRoot;
            var tcinfoHIDbytes = HeapNode.GetHIDBytes(tcinfoHID);
            TCHeader = new TCINFOHEADER(tcinfoHIDbytes.Data);

            RowIndexBTH = new BTH(HeapNode,TCHeader.RowIndexLocation);
            ReverseRowIndex = new Dictionary<uint, uint>();
            foreach(var prop in RowIndexBTH.Properties)
            {
                var temp = BitConverter.ToUInt32(prop.Value.Data, 0);
                ReverseRowIndex.Add(temp,BitConverter.ToUInt32(prop.Key, 0));
            }
            RowMatrix = new TCRowMatrix(this, RowIndexBTH);
        }

        public TableContext(NodeDataDTO nodeData)
        {
            NodeData = nodeData;
            HeapNode = new HN(NodeData);

            var tcinfoHID = HeapNode.HeapNodes[0].Header.UserRoot;
            var tcinfoHIDbytes = HeapNode.GetHIDBytes(tcinfoHID);
            TCHeader = new TCINFOHEADER(tcinfoHIDbytes.Data);

            RowIndexBTH = new BTH(HeapNode, TCHeader.RowIndexLocation);
            ReverseRowIndex = new Dictionary<uint, uint>();
            foreach (var prop in RowIndexBTH.Properties)
            {
                var temp = BitConverter.ToUInt32(prop.Value.Data, 0);
                ReverseRowIndex.Add(temp, BitConverter.ToUInt32(prop.Key, 0));
            }
            RowMatrix = new TCRowMatrix(this, RowIndexBTH);
        }
    }
}
