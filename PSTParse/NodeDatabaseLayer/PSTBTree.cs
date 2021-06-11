namespace PSTParse.NodeDatabaseLayer
{
    public class PSTBTree
    {
        public BTPage Root { get; }

        public PSTBTree(BREF bref, PSTFile pst)
        {
            using (var viewer = pst.PSTMMF.CreateViewAccessor((long)bref.IB, 512))
            {
                var data = new byte[512];
                viewer.ReadArray(0, data, 0, 512);
                Root = new BTPage(data, bref, pst);
            }
        }
    }
}
