#if WinRT

namespace System.Reflection
{
    [Flags]
    public enum BindingFlags
    {
        Default = 0,
        DeclaredOnly = 2,
        Instance = 4,
        Static = 8,
        Public = 0x10,
        NonPublic = 0x20,
    }
}

#endif