﻿// using PSTParse.Utilities;
// using System;

// namespace PSTParse.MessageLayer
// {
//     public class NAMEID
//     {
//         public uint PropertyID;
//         public bool PropertyIDStringOffset;
//         public Guid Guid;
//         public ushort PropIndex;

//         public NAMEID(byte[] bytes, int offset, NamedToPropertyLookup lookup)
//         {
//             this.PropertyID = BitConverter.ToUInt32(bytes, offset);
//             this.PropertyIDStringOffset = (bytes[offset + 4] & 0x1) == 1;
//             var guidType = BitConverter.ToUInt16(bytes, offset + 4) >>1;
//             if (guidType == 1)
//             {
//                 this.Guid = new Guid("00020328-0000-0000-C000-000000000046");//PS-MAPI
//             } else if (guidType == 2)
//             {
//                 this.Guid = new Guid("00020329-0000-0000-C000-000000000046");//PS_PUBLIC_STRINGS
//             } else
//             {
//                 this.Guid = new Guid(lookup.GUIDs.RangeSubset((guidType - 3)*16, 16));
//             }

//             this.PropIndex = (ushort)(0x8000 + BitConverter.ToUInt16(bytes, offset + 6));
//         }
//     }
// }
