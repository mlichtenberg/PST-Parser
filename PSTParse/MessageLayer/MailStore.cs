#nullable enable

using System.Text;
using PSTParse.ListsTablesPropertiesLayer;
using PSTParse.NodeDatabaseLayer;

namespace PSTParse.MessageLayer
{
    public class MailStore
    {
        private PropertyContext _pc;

        public EntryID RootFolder { get; }
        public string? DisplayName { get; }

        public MailStore(PSTFile pst)
        {
            _pc = new PropertyContext(SpecialNIDs.NID_MESSAGE_STORE, pst);
            RootFolder = new EntryID(_pc.Properties[MessageProperty.RootFolder].Data);

            _pc.Properties.TryGetValue(MessageProperty.DisplayName, out ExchangeProperty? displayProp);
            if (displayProp?.Data != null)
            {
                DisplayName = Encoding.Unicode.GetString(displayProp.Data);
            }
        }
    }
}
