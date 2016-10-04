namespace GZipTest.Models
{
    internal class Chunk
    {
        public readonly byte[] Data;
        public readonly long Offset;

        public Chunk(byte[] data, long offset)
        {
            Data = data;
            Offset = offset;
        }
    }
}