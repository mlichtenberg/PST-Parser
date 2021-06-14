using PSTParse.ListsTablesPropertiesLayer;
using PSTParse.NodeDatabaseLayer;

namespace PSTParse.MessageLayer
{
    public class MailStore
    {
        private PropertyContext _pc;

        public EntryID RootFolder { get; }

        public MailStore(PSTFile pst)
        {
            _pc = new PropertyContext(SpecialNIDs.NID_MESSAGE_STORE, pst);
            //RootFolder = new EntryID(_pc.BTH.GetExchangeProperties()[MessageProperty.RootFolder].Data);
            RootFolder = new EntryID(_pc.Properties[MessageProperty.RootFolder].Data);
        }
    }
}
