#if WinRT

namespace NUnit.Framework 
{
    public static class Path
    {
        public static readonly char AltDirectorySeparatorChar = '/';
        public static readonly char DirectorySeparatorChar = '\\';
    }
}

#endif