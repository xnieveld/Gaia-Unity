using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential)]
public struct Star
{
    public UInt64 morton_index;
    public UInt64 id;
    public double x;
    public double y;
    public double z;
    public UInt32 colour;
    public float brightness;
}

[StructLayout(LayoutKind.Sequential)]
public struct DB_CTX
{
    public IntPtr dbp;
    public IntPtr sdbp;
    public string directory;
}

public class GaiaDB
{
    // Return DB_CTX* 
    [DllImport("libgaiadb", EntryPoint = "gaia_setup_database", CharSet = CharSet.Unicode)]
    private static extern IntPtr gaia_setup_database(string directory);

    // Parameter DB_CTX*
    [DllImport("libgaiadb", EntryPoint = "gaia_close_database", CharSet = CharSet.Unicode)]
    public static extern int CloseDB(IntPtr context);

    // DB* dbp
    //[DllImport("gaiadb", EntryPoint = "gaia_new_star", CharSet = CharSet.Unicode)]
    //private static extern int NewStar(IntPtr dbp, UInt64 id, double x, double y, double z,
    //                                    UInt32 colour, float brightness, UInt64 morton_index);

    // DB* dbp, return Star*
    [DllImport("libgaiadb", EntryPoint = "gaia_get_star", CharSet = CharSet.Unicode)]
    private static extern IntPtr GaiaGetStar(IntPtr dbp, UInt64 id);

    // DB* sdbp, return Star* 
    [DllImport("libgaiadb", EntryPoint = "gaia_get_star_by_morton", CharSet = CharSet.Unicode)]
    private static extern IntPtr GaiaGetStarByMorton(IntPtr sdbp, UInt64 morton_idx);

    // DB* dbp
    [DllImport("libgaiadb", EntryPoint = "gaia_delete_star", CharSet = CharSet.Unicode)]
    public static extern int DeleteStar(IntPtr dbp, UInt64 id);

    //[DllImport("gaiadb", EntryPoint = "gaia_update_star_morton", CharSet = CharSet.Unicode)]
    //public static extern IntPtr UpdateStarMorton(string directory);

    // DB* dbp, return DBC*
    [DllImport("libgaiadb", EntryPoint = "gaia_get_cursor", CharSet = CharSet.Unicode)]
    public static extern IntPtr GetCursor(IntPtr dbp);

    // DBC* cursor, return Star*
    [DllImport("libgaiadb", EntryPoint = "gaia_get_next_star", CharSet = CharSet.Unicode)]
    private static extern IntPtr GaiaGetNextStar(IntPtr cursor);

    // DBC* cursor, return Star*
    [DllImport("libgaiadb", EntryPoint = "gaia_goto_star", CharSet = CharSet.Unicode)]
    private static extern IntPtr GaiaGotoStar(IntPtr cursor, UInt64 id);

    // DBC* cursor
    [DllImport("libgaiadb", EntryPoint = "gaia_close_cursor", CharSet = CharSet.Unicode)]
    public static extern int DestroyCursor(IntPtr cursor);

    public static DB_CTX SetupContext()
    {
        IntPtr ctxp = gaia_setup_database("");
        return (DB_CTX)Marshal.PtrToStructure(ctxp, typeof(DB_CTX));
    }

    public static Star GetStar(DB_CTX context, ulong id)
    {
        IntPtr starp = GaiaGetStar(context.dbp, id);
        return (Star)Marshal.PtrToStructure(starp, typeof(Star)); 
    }

    public static Star GetStarByMorton(DB_CTX context, ulong idx)
    {
        return (Star)Marshal.PtrToStructure(GaiaGetStarByMorton(context.dbp, idx), typeof(Star));
    }

    public static Star GetNextStar(IntPtr cursor)
    {
        return (Star)Marshal.PtrToStructure(GaiaGetNextStar(cursor), typeof(Star));
    }

    public static Star GotoStar(IntPtr cursor, ulong id)
    {
        return (Star)Marshal.PtrToStructure(GaiaGotoStar(cursor, id), typeof(Star));
    }
}
