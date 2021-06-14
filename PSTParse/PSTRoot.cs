using System;

namespace PSTParse
{
    public class PSTRoot
    {
        public uint D_W_Reserved { get; set; }
        /// <summary>
        /// The size of the PST file, in bytes.
        /// </summary>
        public ulong FileSizeBytes { get; }
        public byte F_A_MapValid { get; set; }

        public PSTRoot(byte[] rootBuffer)
        {
            D_W_Reserved = BitConverter.ToUInt32(rootBuffer, 0);
            FileSizeBytes = BitConverter.ToUInt64(rootBuffer, 4);
            F_A_MapValid = rootBuffer[68];
        }
    }
}
