namespace GZipTest.Models
{
    using System;
    using System.IO;

    internal class Chunk
    {
        public readonly byte[] Data;
        public readonly long Offset;

        public Chunk(byte[] data, long offset)
        {
            Data = data;
            Offset = offset;
        }

        //                          Structure Header
        //
        //
        //        original offset    gzip block lenght    gzip compress block
        //      +------------------+-------------------+----------------------+
        //      |000010000000000000|0000001000000000000|...compressed block...|
        //      +------------------+-------------------+----------------------+
        //
        // NOTE: Extract from this class
        internal void Write(Stream output)
        {
            output.Write(GetBytes(Offset), 0, 8);
            output.Write(GetBytes(Data.LongLength), 0, 8);
            output.Write(Data, 0, Data.Length);
        }

        private byte[] GetBytes(long number) => BitConverter.GetBytes(number);
    }
}