using System;

namespace WinApi.User32
{
    public enum ResourceImageType
    {
        IMAGE_BITMAP = 0,
        IMAGE_ICON = 1,
        IMAGE_CURSOR = 2
    }

    [Flags]
    public enum LoadResourceFlags
    {
        LR_DEFAULTCOLOR = 0,
        LR_MONOCHROME = 1,
        LR_LOADFROMFILE = 16,
        LR_LOADTRANSPARENT = 32,
        LR_DEFAULTSIZE = 64,
        LR_VGACOLOR = 128,
        LR_LOADMAP3DCOLORS = 4096,
        LR_CREATEDIBSECTION = 8192,
        LR_SHARED = 32768
    }

    [Flags]
    public enum WindowClassStyles
    {
        CS_VREDRAW = 1,
        CS_HREDRAW = 2,
        CS_DBLCLKS = 8,
        CS_OWNDC = 32,
        CS_CLASSDC = 64,
        CS_PARENTDC = 128,
        CS_NOCLOSE = 512,
        CS_SAVEBITS = 2048,
        CS_BYTEALIGNCLIENT = 4096,
        CS_BYTEALIGNWINDOW = 8192,
        CS_GLOBALCLASS = 16384,
        CS_DROPSHADOW = 131072
    }
}
