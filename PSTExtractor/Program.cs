using PSTParse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

Settings settings = ReadCommandLine();
if (settings.isValid)
{
    var sw = new Stopwatch();
    sw.Start();

    var pstPath = settings.pstPath;
    var outputRootFolder = settings.outputRootFolder;

    var fileInfo = new FileInfo(pstPath);
    var pstSizeGigabytes = ((double)fileInfo.Length / 1000 / 1000 / 1000).ToString("0.000");
    if (!Directory.Exists(outputRootFolder)) Directory.CreateDirectory(outputRootFolder);

    using (var file = new PSTFile(pstPath))
    {
        var stack = new Stack<MailFolder>();
        stack.Push(file.TopOfPST);

        var totalCount = 0;
        var skippedFolders = new List<string>();

        var outputFolder = string.Empty;
        while (stack.Count > 0)
        {
            var curFolder = stack.Pop();

            outputFolder = string.Format("{0}\\{1}", outputRootFolder, string.Join("\\", curFolder.Path));
            if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

            foreach (var child in curFolder.SubFolders)
            {
                stack.Push(child);
            }

            if (curFolder.Path.Count > 1 && curFolder.ContainerClass != "" && curFolder.ContainerClass != "IPF.Note")
            {
                var count = curFolder.Count;
                var line = $"{string.Join(" -> ", curFolder.Path)}({curFolder.ContainerClass}) ({count} messages)";
                skippedFolders.Add(line);
                continue;
            }

            foreach (var message in curFolder.GetIpmNotes())
            {
                RenderMessage(message, outputFolder);
                totalCount++;
            }
        }

        sw.Stop();
        var elapsedSeconds = (double)sw.ElapsedMilliseconds / 1000;
        Console.WriteLine("{0} messages total", totalCount);
        Console.WriteLine("Parsed {0} ({1} GB) in {2:0.00} seconds", Path.GetFileName(pstPath), pstSizeGigabytes, elapsedSeconds);

        if (skippedFolders.Count > 0)
        {
            Console.WriteLine("\r\nSkipped Folders:\r\n");
            foreach (var line in skippedFolders)
            {
                Console.WriteLine(line);
            }
        }
    }
}
else
{
    Console.WriteLine("ERROR: Invalid command line arguments");
    Console.WriteLine("Usage:  PSTExtractor [PATH_TO_PST] [OUTPUT_FOLDER]");
    Console.WriteLine("If [OUTPUT_FOLDER] does not exist, it will be created.");
}

static void RenderMessage(PSTParse.MessageLayer.Message message, string outputFolder)
{
    // Build a string containing the email message metadata
    StringWriter sw = new StringWriter();
    sw.WriteLine("Date: {0}", message.ClientSubmitTime.ToShortDateString() + " " + message.ClientSubmitTime.ToShortTimeString());
    sw.WriteLine("Subject: {0}", message.SubjectPrefix + message.Subject);
    sw.WriteLine("From: {0}", message.SenderName);
    if (message.Recipients.To.Count > 0)
    {
        sw.WriteLine("To: {0}", String.Join("; ", message.Recipients.To.Select(r => string.IsNullOrWhiteSpace(r.DisplayName) ? r.EmailAddress : r.DisplayName)));
    }
    if (message.Recipients.CC.Count > 0)
    {
        sw.WriteLine("CC: {0}", String.Join("; ", message.Recipients.CC.Select(r => string.IsNullOrWhiteSpace(r.DisplayName) ? r.EmailAddress : r.DisplayName)));
    }
    if (message.Recipients.BCC.Count > 0)
    {
        sw.WriteLine("BCC: {0}", String.Join("; ", message.Recipients.BCC.Select(r => string.IsNullOrWhiteSpace(r.DisplayName) ? r.EmailAddress : r.DisplayName)));
    }
    sw.WriteLine();
    if (message.BodyHtml != null)
    {        
        // Not perfect, but good enough for now
        var bodyHtml = message.BodyHtml;
        bodyHtml = bodyHtml
            .Replace("<br>", "\n")
            .Replace("<br/>", "\n")
            .Replace("</p>", "\n\r")
            .Replace("&amp;", "&")
            .Replace("&nbsp;", " ")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Replace("&quot;", "'");
        bodyHtml = Regex.Replace(bodyHtml, "(?=<!--)([\\s\\S]*?)-->", string.Empty);   // Remove HTML comments
        bodyHtml = Regex.Replace(bodyHtml, "<.*?>", string.Empty);  // Remove remaining HTML tags
        sw.WriteLine(bodyHtml);
    }
    else if (message.BodyPlainText != null)
    {
        sw.WriteLine(message.BodyPlainText);
    }
    else if (message.BodyCompressedRTFString != null)
    {
        // TODO:  Convert compressed RTF to plain text
        // Consider https://stackoverflow.com/questions/5634525/how-to-convert-an-rtf-string-to-text-in-c-sharp
        sw.WriteLine(message.BodyCompressedRTFString);
    }
    sw.WriteLine();

    // Build the filename for the email message
    var fileNameRoot = string.Format("{0}-{1}-{2}",
        message.ClientSubmitTime.ToString("yyyyMMdd-HHmm"), message.SenderName, message.Subject);
    fileNameRoot = GetValidFilename(fileNameRoot);
    var messageFileName = string.Format("{0}\\{1}.txt", outputFolder, fileNameRoot);

    // Write the email message to a file
    if (!File.Exists(messageFileName)) File.AppendAllText(messageFileName, sw.ToString());

    // If the email message has attachments, write each of them to a file
    foreach (var attachment in message.Attachments)
    {
        if (attachment.AttachmentLongFileName != null)
        {
            if (!Directory.Exists(string.Format("{0}\\attachments\\{1}", outputFolder, fileNameRoot)))
            {
                Directory.CreateDirectory(string.Format("{0}\\attachments\\{1}", outputFolder, fileNameRoot));
            }
            var attachmentFileName = string.Format("{0}\\attachments\\{1}\\{2}", outputFolder, fileNameRoot, attachment.AttachmentLongFileName);
            if (!File.Exists(attachmentFileName)) File.WriteAllBytes(attachmentFileName, attachment.Data);
        }
    }
}

static string GetValidFilename(string fileName)
{
    foreach (char c in Path.GetInvalidFileNameChars())
    {
        fileName = fileName.Replace(c, '_');
    }
    // Remove all periods from filename (mostly to prevent periods at the end of the filename)
    fileName = fileName.Replace(".", "");
    return fileName.Substring(0, fileName.Length > 100 ? 100 : fileName.Length).Trim();
}

static Settings ReadCommandLine()
{
    Settings settings = new Settings();
    if (Environment.GetCommandLineArgs().Length == 3)
    {
        settings.isValid = true;
        settings.pstPath = Environment.GetCommandLineArgs()[1];
        settings.outputRootFolder = Environment.GetCommandLineArgs()[2];
    }
    return settings;
}

public class Settings
{
    public bool isValid { get; set; } = false;
    public string pstPath { get; set; } = string.Empty;
    public string outputRootFolder { get; set; } = string.Empty;
}