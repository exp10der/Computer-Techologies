namespace GZipTest
{
    using Files;

    internal class Program
    {
        //  C:\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\MachineName\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\?\C:\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\?\UNC\MachineName\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\127.0.0.1\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\?\UNC\127.0.0.1\Work\Node.js Design Patterns - Second Edition.pdf


        private static void Main(string[] args)
        {
            using (var input = File.OpenRead(@"\\?\C:\Work\Node.js Design Patterns - Second Edition.pdf", 4096, false))
            using (var output = File.OpenWrite(@"\\?\C:\Work\Node.js Design Patterns - Second Edition.gz", 4096, false))
            {
                var compressor = StreamChunk.SplitStream(input, 64 * 1024).Transform(chunk => Compress(chunk));

                foreach (var compressedChunk in compressor)
                {
                    compressedChunk.Write(output);
                }
            }


        }
    }
}