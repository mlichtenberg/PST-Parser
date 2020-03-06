using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using PSTParse.MessageLayer;
using PSTParse.NodeDatabaseLayer;
using PSTParse.ListsTablesPropertiesLayer;
using System.Linq;
using PSTParse.Utilities;

namespace PSTParse
{
    public class PSTFile : IDisposable
    {
        public const int MinFileSizeBytes = 1_000;

        public string Path { get; }
        public MemoryMappedFile PSTMMF { get; private set; }
        public PSTHeader Header { get; }
        public MailStore MailStore { get; }
        public MailFolder TopOfPST { get; }
        //public NamedToPropertyLookup NamedPropertyLookup { get; }
        public double SizeMB => (double)Header.Root.FileSizeBytes / 1000 / 1000;

        public PSTFile(string path)
        {
            if (new FileInfo(path).Length < MinFileSizeBytes)
            {
                throw new Exception($"Failed opening PST, file size must be greater than {MinFileSizeBytes} bytes");
            }
            Path = path ?? throw new ArgumentNullException(nameof(path));
            PSTMMF = MemoryMappedFile.CreateFromFile(path, FileMode.Open);

            Header = new PSTHeader(this);
            if (!Header.IsUNICODE && !Header.IsANSI) throw new InvalidDataException("PST is not a valid data file");
            if (!Header.IsUNICODE) throw new InvalidDataException("PST Parser currently only supports UNICODE");

            /*var messageStoreData = BlockBO.GetNodeData(SpecialNIDs.NID_MESSAGE_STORE);
            var temp = BlockBO.GetNodeData(SpecialNIDs.NID_ROOT_FOLDER);*/
            MailStore = new MailStore(this);

            TopOfPST = new MailFolder(MailStore.RootFolder.NID, new List<string>(), this);
            //NamedPropertyLookup = new NamedToPropertyLookup(this);

            //var temp = new TableContext(rootEntryID.NID);
        }

        public bool IsPasswordProtected()
        {
            var messageStore = new PropertyContext(SpecialNIDs.NID_MESSAGE_STORE, this);
            var rootDataNode = messageStore.BTH.Root.Data;
            const int unknown2Bytes = 2;
            var passwordKey = new byte[] { 0xFF, 0x67 };
            foreach (var entry in rootDataNode.DataEntries)
            {
                if (entry.Key.SequenceEqual(passwordKey))
                {
                    var dataBlockOffset = (int)entry.DataOffset + (int)rootDataNode.Data.BlockOffset + unknown2Bytes;
                    var slice = rootDataNode.Data.Parent.Data.Skip(dataBlockOffset).Take(4).ToList();
                    var isProtected = !slice.SequenceEqual(new byte[] { 0, 0, 0, 0 });
                    return isProtected;
                }
            }
            return false;
        }

        public bool RemovePassword()
        {
            var messageStore = new PropertyContext(SpecialNIDs.NID_MESSAGE_STORE, this);
            var rootDataNode = messageStore.BTH.Root.Data;
            const int unknown2Bytes = 2;
            var passwordKey = new byte[] { 0xFF, 0x67 };
            foreach (var entry in rootDataNode.DataEntries)
            {
                if (entry.Key.SequenceEqual(passwordKey))
                {
                    var dataBlockOffset = (int)entry.DataOffset + (int)rootDataNode.Data.BlockOffset + unknown2Bytes;
                    var slice = rootDataNode.Data.Parent.Data.Skip(dataBlockOffset).Take(4).ToList();
                    var isProtected = !slice.SequenceEqual(new byte[] { 0, 0, 0, 0 });
                    if (!isProtected) return false;

                    CloseMMF();

                    using (var stream = new FileStream(Path, FileMode.Open))
                    {
                        rootDataNode.Data.Parent.Data[dataBlockOffset] = 0x00;
                        rootDataNode.Data.Parent.Data[dataBlockOffset + 1] = 0x00;
                        rootDataNode.Data.Parent.Data[dataBlockOffset + 2] = 0x00;
                        rootDataNode.Data.Parent.Data[dataBlockOffset + 3] = 0x00;

                        DatatEncoder.CryptPermute(rootDataNode.Data.Parent.Data, rootDataNode.Data.Parent.Data.Length, true, Header.EncodingAlgotihm);

                        // seems to always be [65, 65, 65, 65]
                        var permutationBytes = rootDataNode.Data.Parent.Data.Skip(dataBlockOffset).Take(4).ToArray();
                        stream.Seek((long)rootDataNode.Data.Parent.PstOffset + dataBlockOffset, SeekOrigin.Begin);
                        stream.Write(permutationBytes, 0, 4);

                        var newCRC = new CRC32().ComputeCRC(0, rootDataNode.Data.Parent.Data, (uint)rootDataNode.Data.Parent.Data.Length);
                        DatatEncoder.CryptPermute(rootDataNode.Data.Parent.Data, rootDataNode.Data.Parent.Data.Length, false, Header.EncodingAlgotihm);
                        var crcoffset = (long)(rootDataNode.Data.Parent.PstOffset + rootDataNode.Data.Parent.CRCOffset);
                        stream.Seek(crcoffset, SeekOrigin.Begin);
                        var crcBuffer = BitConverter.GetBytes(newCRC);
                        stream.Write(crcBuffer, 0, 4);
                    }
                    OpenMMF();
                    return true;
                }
            }
            return false;
        }

        public void CloseMMF()
        {
            PSTMMF.Dispose();
        }

        public void OpenMMF()
        {
            PSTMMF = MemoryMappedFile.CreateFromFile(Path, FileMode.Open);
        }

        public Tuple<ulong, ulong> GetNodeBIDs(ulong NID)
        {
            return Header.NodeBT.Root.GetNIDBID(NID);
        }

        public void Dispose()
        {
            CloseMMF();
        }

        public BBTENTRY GetBlockBBTEntry(ulong item1)
        {
            return Header.BlockBT.Root.GetBIDBBTEntry(item1);
        }
    }
}
