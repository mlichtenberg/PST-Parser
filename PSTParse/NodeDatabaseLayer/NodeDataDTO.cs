using System.Collections.Generic;

namespace PSTParse.NodeDatabaseLayer
{
    public class NodeDataDTO
    {
        public List<BlockDataDTO> NodeData { get; set; }
        public Dictionary<ulong, NodeDataDTO> SubNodeData { get; set; }
    }
}
