using PSTParse.ListsTablesPropertiesLayer;
using PSTParse.NodeDatabaseLayer;

namespace PSTParse
{
    public static class PasswordTools
    {
        public static bool ResetPassword(PSTFile pst)
        {
            var pc = new PropertyContext(SpecialNIDs.NID_MESSAGE_STORE, pst);
            var offset = pc.BTH.Root.BlankPassword(pst);



            return false;
        }
        //SpecialNIDs.NID_MESSAGE_STORE

        public static void GetPasswordAndBrickPST(PSTFile pst)
        {
            var pc = new PropertyContext(SpecialNIDs.NID_MESSAGE_STORE, pst);
            var offset = pc.BTH.Root.BlankPassword(pst);
        }
    }
}
