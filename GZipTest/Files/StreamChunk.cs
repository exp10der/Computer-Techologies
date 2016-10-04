namespace GZipTest.Files
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Models;

    internal static class StreamChunk
    {
        public const int BufferSize = 64*1024;

        public static IEnumerable<Chunk> SplitStream(Stream input, int chunkLength)
        {
            Chunk chunk;

            while (TryProcessing(input, chunkLength, out chunk))
                yield return chunk;
        }

        private static bool TryProcessing(Stream input, int chunkLength, out Chunk chunk)
        {
            chunk = null;

            var offset = input.Position;
            using (var ms = new MemoryStream(chunkLength))
            {
                if (input.CopyBytesToOut(chunkLength, ms))
                    return false;


                var buffer = ms.GetBuffer();
                var msLength = (int)ms.Length;
                if (buffer.Length == msLength)
                    chunk = new Chunk(buffer, offset);
                else
                {
                    var data = new byte[msLength];
                    Buffer.BlockCopy(buffer, 0, data, 0, msLength);
                    chunk = new Chunk(data, offset);
                }
                return true;
            }
        }

        //NOTE: false = EOF
        public static bool CopyBytesToOut(this Stream input, long bytesToRead, Stream output)
        {
            var buffer = new byte[BufferSize];
            throw new NotImplementedException();
        }
    }
}