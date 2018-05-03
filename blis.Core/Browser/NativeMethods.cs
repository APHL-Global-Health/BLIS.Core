using System;
using System.Runtime.InteropServices;

namespace blis.Core.Browser
{
    internal static class NativeMethods
    {
        [DllImport("libgdk-x11-2.0.so.0", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_x11_drawable_get_xid(IntPtr raw);

        [DllImport("libgdk-win32-2.0-0.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr gdk_win32_drawable_get_handle(IntPtr raw);
    }
}
