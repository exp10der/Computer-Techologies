namespace GZipTest.Files.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;

    // ReSharper disable once InconsistentNaming
    internal class WinNT
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string filename,
            uint desiredAccess,
            uint shareMode,
            IntPtr attributes,
            uint creationDisposition,
            uint flagsAndAttributes,
            IntPtr templateFile);

        internal const int FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
    }
}