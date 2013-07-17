#if WinRT

namespace NUnit.Framework 
{
    public enum PlatformID
    {
        Win32S = 0,
        Win32Windows = 1,
        Win32NT = 2,
        WinCE = 3,
        Unix = 4,
        Xbox = 5,
        MacOSX = 6,
        WinRT = 7,
        UnixMono = 128,
    }
}

#endif