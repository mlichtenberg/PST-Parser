using System;
using System.Text;
using PSTParse.NodeDatabaseLayer;

namespace PSTParse
{
    public class PSTHeader
    {
        public string DWMagic { get; }
        public PstVersion Version { get; }
        public PSTBTree NodeBT { get; private set; }
        public PSTBTree BlockBT { get; private set; }
        public BlockEncoding EncodingAlgotihm { get; private set; }
        public PSTRoot Root { get; }

        public PSTHeader(PSTFile pst)
        {
            using (var mmfView = pst.PSTMMF.CreateViewAccessor(0, 684))
            {
                var dwMagicBuffer = new byte[4];
                mmfView.ReadArray(0, dwMagicBuffer, 0, 4);
                DWMagic = Encoding.Default.GetString(dwMagicBuffer);

                var ver = mmfView.ReadInt16(10);
                Version = ver == 23 ? PstVersion.UNICODE : PstVersion.ANSI;
                if (Version == PstVersion.ANSI)
                {
                    throw new Exception("ANSI encoded PST not supported");
                }

                var rootBuffer = new byte[72];
                mmfView.ReadArray(180, rootBuffer, 0, rootBuffer.Length);

                Root = new PSTRoot(rootBuffer);

                var sentinel = mmfView.ReadByte(512);
                var cryptMethod = (uint)mmfView.ReadByte(513);

                EncodingAlgotihm = (BlockEncoding)cryptMethod;

                var bytes = new byte[16];
                mmfView.ReadArray(216, bytes, 0, 16);
                var nbt_bref = new BREF(bytes);

                mmfView.ReadArray(232, bytes, 0, 16);
                var bbt_bref = new BREF(bytes);

                NodeBT = new PSTBTree(nbt_bref, pst);
                BlockBT = new PSTBTree(bbt_bref, pst);
            }
        }

        public enum BlockEncoding
        {
            NONE = 0,
            PERMUTE = 1,
            CYCLIC = 2
        }

        public enum PstVersion
        {
            ANSI,
            UNICODE,
        }
    }
}
