namespace GZipTest.Infrastructure
{
    using System.IO;
    using System.IO.Compression;
    using Models;

    internal class Compressor
    {
        public static Chunk Compress(Chunk chunk)
        {
            using (var inputMs = new MemoryStream(chunk.Data))
            using (var outputMs = new MemoryStream())
            using (var gz = new GZipStream(outputMs, CompressionMode.Compress))
            {
                var buffer = new byte[32768];
                int read;
                while ((read = inputMs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    gz.Write(buffer, 0, read);
                }
                return new Chunk(outputMs.ToArray(), chunk.Offset);
            }
        }
    }
}