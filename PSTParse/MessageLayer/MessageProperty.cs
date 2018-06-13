﻿using System;

namespace PSTParse.MessageLayer
{
    public enum MessageProperty : uint
    {
        GuidList = 0x2,
        EntryList = 0x3,
        StringList = 0x4,
        Importance = 0x17,
        MessageClass = 0x1a,
        DeliveryReportRequested = 0x23,
        Priority = 0x26,
        ReadReceiptRequested = 0x29,
        RecipientReassignmentProhibited = 0x2B,
        SensitivityOriginal = 0x2E,
        ReportTime = 0x32,
        Sensitivity = 0x36,
        Subject = 0x37,
        ClientSubmitTime = 0x39,
        OriginalSenderWithScheme = 0x3b,
        ReceivedByEntryID = 0x3F,
        ReceivedByName = 0x40,
        SentRepresentingEntryID = 0x41,
        SentRepresentingName = 0x42,
        ReceivedRepresentingEntryID = 0x43,
        ReceivedRepresentingName = 0x44,
        ReplyRecipientEntries = 0x4F,
        ReplyRecipientNames = 0x50,
        ReceivedBySearchKey = 0x51,
        ReceivedRepresentingSearchKey = 0x52,
        MessageToMe = 0x57,
        MessageCCMe = 0x58,
        MessageRecipientMe = 0x59,
        ResponseRequested = 0x60,
        SentRepresentingAddressType = 0x64,
        SentRepresentingAddress = 0x65,
        ConversationTopic = 0x70,
        ConversationIndex = 0x71,
        OriginalDisplayBcc = 0x72,
        OriginalDisplayCc = 0x73,
        OriginalDisplayTo = 0x74,
        ReceivedByAddressType = 0x75,
        ReceivedByAddress = 0x76,
        ReceivedRepresentingAddressType = 0x77,
        ReceivedRepresentingAddress = 0x78,
        Headers = 0x7d,
        UserEntryID = 0x619,
        NdrReasonCode = 0xC04,
        NdrDiagCode = 0xC05,
        NonReceiptNotificationRequested = 0xC06,
        RecipientType = 0xc15,
        ReplyRequested = 0xc17,
        SenderEntryID = 0xc19,
        SenderName = 0xc1a,
        SupplementaryInfo = 0xc1b,
        SenderSearchKey = 0xc1d,
        SenderAddressType = 0xc1e,
        SenderAddress = 0xc1f,
        DeleteAfterSubmit = 0xe01,
        DisplayBccAddresses = 0xe02,
        DisplayCcAddresses = 0xe03,
        RecipientName = 0xe04,
        MessageDeliveryTime = 0xe06,
        MessageFlags = 0xe07,
        MessageSize = 0xe08,
        SentMailEntryID = 0xe0a,
        RecipientResponsibility = 0xe0f,
        NormalizedSubject = 0xe1d,
        RtfInSync = 0xe1f,
        AttachmentSize = 0xe20,
        InternetArticleNumber = 0xe23,
        NextSentAccount = 0xe29,
        TrustedSender = 0xe79,
        RecordKey = 0xff9,
        RecipientObjType = 0xffe,
        RecipientEntryID = 0xfff,
        BodyPlainText = 0x1000,
        ReportText = 0x1001,
        BodyRtfCrc = 0x1006,
        BodyRtfSyncCount = 0x1007,
        BodyRtfSyncTag = 0x1008,
        BodyCompressedRTF = 0x1009,
        BodyRtfSyncPrefixCount = 0x1010,
        BodyRtfSyncTrailingCount = 0x1011,
        BodyHtml = 0x1013,
        MessageID = 0x1035,
        ReferencesMessageID = 0x1039,
        ReplyToMessageID = 0x1042,
        UnsubscribeAddress = 0x1045,
        ReturnPath = 0x1046,
        UrlCompositeName = 0x10F3,
        AttributeHidden = 0x10F4,
        ReadOnly = 0x10F6,
        DisplayName = 0x3001,
        AddressType = 0x3002,
        AddressName = 0x3003,
        Comment = 0x3004,
        CreationTime = 0x3007,
        LastModificationTime = 0x3008,
        SearchKey = 0x300B,
        ValidFolderMask = 0x35df,
        RootFolder = 0x35e0,
        OutboxFolder = 0x35e2,
        DeletedItemsFolder = 0x35e3,
        SentFolder = 0x35e4,
        UserViewsFolder = 0x35e5,
        CommonViewsFolder = 0x35e6,
        SearchFolder = 0x35e7,
        FolderContentCount = 0x3602,
        FolderUnreadCount = 0x3603,
        FolderHasChildren = 0x360a,
        ContainerClass = 0x3613,
        AssocContentCount = 0x3617,
        AttachmentData = 0x3701,
        AttachmentFileName = 0x3704,
        AttachmentMethod = 0x3705,
        AttachmentLongFileName = 0x3707,
        AttachmentRenderPosition = 0x370b,
        AttachmentMimeType = 0x370e,
        AttachmentMimeSequence = 0x3710,
        AttachmentContentID = 0x3712,
        AttachmentFlags = 0x3714,
        CodePage = 0x3fDE,
        CreatorName = 0x3ff8,
        NonUnicodeCodePage = 0x3ffd,
        LocaleID = 0x3ff1,
        CreatorEntryID = 0x3ff9,
        LastModifierName = 0x3ffa,
        LastModifierEntryID = 0x3ffb,
        SentRepresentingFlags = 0x401a,
        BodyPlainText2 = 0x6619,
        AttachmentLTPRowID = 0x67F2,
        AttachmentLTPRowVer = 0x67F3,
        BodyPlainText3 = 0x8008,
        ContentClass = 0x8009,
        PopAccountName = 0x800d,
        PopUri = 0x8011,
        ContentType2 = 0x8013,
        TransferEncoding2 = 0x8014,
        BodyPlainText4 = 0x8015,
        PopUri2 = 0x804c,
        PopServerName = 0x8070,
        ContentType = 0x8076,
        TransferEncoding = 0x807b,
        BodyPlainText5 = 0x807e,
        MailSoftwareName = 0x8088,
        PopAccountName2 = 0x808a,
        MailSoftwareEngine = 0x808b,
    }
}