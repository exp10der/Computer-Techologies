namespace GZipTest.Files
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.Win32.SafeHandles;
    using Win32;

    internal class File
    {
        public static FileStream OpenRead(string path, int bufferSize, bool sequential)
        {
            return OpenRead(path, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, sequential);
        }

        private static FileStream OpenRead(string path, FileMode mode, FileAccess access, FileShare share,
            int bufferSize, bool sequential)
        {
            return new FileStream(CreateFileHandle(path, mode, access, share, sequential), access, bufferSize, false);
        }

        private static SafeFileHandle CreateFileHandle(string path, FileMode mode, FileAccess access, FileShare share,
            bool sequential)
        {
            var flags = sequential ? WinNT.FILE_FLAG_SEQUENTIAL_SCAN : 0;

            var fileHandle = WinNT.CreateFile(path, (uint) access, (uint) share, IntPtr.Zero, (uint) mode, (uint) flags,
                IntPtr.Zero);


            if (!fileHandle.IsInvalid) return fileHandle;

            // Check for errors.
            var lastWin32Error = Marshal.GetLastWin32Error();
            throw new Win32Exception(
                lastWin32Error,
                $"Error {lastWin32Error} creating file handle for file path '{path}': {new Win32Exception(lastWin32Error).Message}");
        }
    }
}