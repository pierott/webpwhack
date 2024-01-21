using System.Runtime.InteropServices;

namespace WebpWhack
{
    [Flags]
    public enum ChangeEventFlags
    {
        SHCNF_IDLIST      = 0x0000,        // LPITEMIDLIST
        SHCNF_PATHA       = 0x0001,        // path name
        SHCNF_PRINTERA    = 0x0002,        // printer friendly name
        SHCNF_DWORD       = 0x0003,        // DWORD
        SHCNF_PATHW       = 0x0005,        // path name
        SHCNF_PRINTERW    = 0x0006,        // printer friendly name
        SHCNF_TYPE        = 0x00FF,
        SHCNF_FLUSH       = 0x1000,
        SHCNF_FLUSHNOWAIT = 0x3000,        // includes SHCNF_FLUSH
        SHCNF_NOTIFYRECURSIVE      = 0x10000, // Notify clients registered for any child
    }

    public enum ChangeEventId
    {
        SHCNE_RENAMEITEM          = 0x00000001,
        SHCNE_CREATE              = 0x00000002,
        SHCNE_DELETE              = 0x00000004,
        SHCNE_MKDIR               = 0x00000008,
        SHCNE_RMDIR               = 0x00000010,
        SHCNE_MEDIAINSERTED       = 0x00000020,
        SHCNE_MEDIAREMOVED        = 0x00000040,
        SHCNE_DRIVEREMOVED        = 0x00000080,
        SHCNE_DRIVEADD            = 0x00000100,
        SHCNE_NETSHARE            = 0x00000200,
        SHCNE_NETUNSHARE          = 0x00000400,
        SHCNE_ATTRIBUTES          = 0x00000800,
        SHCNE_UPDATEDIR           = 0x00001000,
        SHCNE_UPDATEITEM          = 0x00002000,
        SHCNE_SERVERDISCONNECT    = 0x00004000,
        SHCNE_UPDATEIMAGE         = 0x00008000,
        SHCNE_DRIVEADDGUI         = 0x00010000,
        SHCNE_RENAMEFOLDER        = 0x00020000,
        SHCNE_FREESPACE           = 0x00040000,
        SHCNE_ALLEVENTS           = 0x7FFFFFFF
    }

    public interface IDesktopNotifier
    {
        void RefreshDesktop();
    }

    public class DesktopNotifier : IDesktopNotifier
    {
        [DllImport( "shell32.dll", CharSet = CharSet.Unicode )]
        public static extern void SHChangeNotify( ChangeEventId wEventId, ChangeEventFlags uFlags, string dwItem1, IntPtr dwItem2 );

        public void RefreshDesktop()
        {
            SHChangeNotify( ChangeEventId.SHCNE_UPDATEDIR, ChangeEventFlags.SHCNF_PATHW, Environment.GetFolderPath( Environment.SpecialFolder.Desktop ), IntPtr.Zero );
        }
    }
}
