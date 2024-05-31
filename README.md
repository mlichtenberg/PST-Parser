# PSTExtractor

  A console application that uses the PSTParse library to extract all messages (including attachments) from a PST mailstore,
  and write them to a set of folders and files that match the folder structure within the PST.
  
## Usage
```
PSTExtractor [PATH_TO_PST] [OUTPUT_FOLDER]
```

## Example

  If a PST file named TEST.pst that is located in the C:\outlook folder has the following structure:
```
  MyPST (root)
    Archive (folder)
	  Old message 1 from Mike at 5/30/2024 09:00AM (email with one .JPG attachment)
	  Old message 2 from Bob at 5/31/2024 01:30PM (email)
	ToDo (folder)
	  Birthday list from Betty at 5/20/2024 5:15PM (email with two .XLSX attachments)
	New resume from Lisa at 5/25/2024 7:30AM (email)  
```

  then running the following command 
  
```
PSTExtractor C:\outlook\TEST.pst c:\PST
```

  will create the following folders and files

```
  C:\PST
    C:\PST\MyPST
      C:\PST\MyPST\Top of Personal folders
        20240525-0730-Lisa-New resume.txt
        C:\PST\MyPST\Top of Personal folders\Archive
          20240530-0900-Mike-Old message 1.txt
          20240531-1330-Bob-Old message 2.txt
          C:\PST\MyPST\Top of Personal folders\Archive\attachments
            C:\PST\MyPST\Top of Personal folders\Archive\attachments\20240530-0900-Mike-Old message 1
              image.JPG
        C:\PST\MyPST\Top of Personal folders\ToDo
          20240520-1715-Betty-Birthday list.txt
          C:\PST\MyPST\Top of Personal folders\ToDo\attachments
            C:\PST\MyPST\Top of Personal folders\ToDo\attachments\20240520-1715-Betty-Birthday list
              List1.XLSX
              List2.XLSX
```  

# PSTParse (Original Documentation)

  A library for reading the [PST mailstore file format.](http://msdn.microsoft.com/en-us/library/ff385210(v=office.12).aspx)

  This library is intended to be as accurate, fast implementation of the PST mailstore file format specification.  The original motivation for this project came from my experiences with other mailstore libraries that either 1) required Outlook to be installed in order to function or 2) were developed inconsistently by a third party.  Such inconsistencies range from libraries that "missed" items and other libraries that failed when encountering errors.  The intention of this project is to provide a basis to developers of applications that need to read and write to the PST format.
  
## PST Structure Overview

  The structure of the PST file format is divided into 3 layers: NDB layer, LTP layer, and the Messaging Layer.  Each layer is implemented on top of the preceding layer.  For example, the LTP layer may implement a heap which is stored on a node in the NDB layer.  Each layer is divided into it's own namespace.  The main entry point of parsing a PST is through the header.  In the header, information about the format and encoding is stored.  The first offsets for the NDB layer are contained Root structure in the header.
  
  The Node Database (NDB) layer layer consists of two [B-trees](http://en.wikipedia.org/wiki/Btree) : one for nodes and another for data blocks.  Each B-tree implementation consists of intermediate blocks and leaf blocks.  The node B-tree consists of nodes that reference block IDs (BIDs) and sub nodes.  BIDs are used to traverse the data block B-tree to resolve to absolute offsets to data streams in the PST.  Data stream themselves can be in one data block or stored in another BTree if the data stream is too large to fit in one page.  XBLOCK and XXBLOCKs structures are used to store the B-trees that are used to store large data streams.
  
  The LTP layer provides the interface for the messaging layer to access properties and variable arrays of content.  The base of the LTP layer is a heap which can be stored on a node (heap-on-node or HN).  On the HN, yet another B-tree (B-tree-on-heap or BTH) is implemented and is used to store values on the HN using keys.  The BTH (can be thought of just as a heap) is used to store Property Contexts (PCs) and Table Contexts (TCs).  
  
  The messaging layer uses the LTP layer to represent folder hierarchies and the messages that exist in a give folder.
  
## Installation
```ps
Install-Package PSTParse
```

## Usage

```c#
using System.Collections.Generic;
using PSTParse;

namespace PSTParseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pstPath = @"path\to\file.pst";
            using (var file = new PSTFile(pstPath))
            {
                var stack = new Stack<MailFolder>();
                stack.Push(file.TopOfPST);
                while (stack.Count > 0)
                {
                    var curFolder = stack.Pop();
                    foreach (var child in curFolder.SubFolders)
                    {
                        stack.Push(child);
                    }

                    foreach (var ipmItem in curFolder.GetIpmItems())
                    {
                        // process item
                    }
                }
            }
        }
    }
}
```
