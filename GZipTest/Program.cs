namespace GZipTest
{
    using System;
    using System.Collections.Generic;
    using Dataflow;
    using Files;
    using Infrastructure;
    using Models;

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
            try
            {
                using (var input = File.OpenRead(@"\\?\C:\Work\Node.js Design Patterns - Second Edition.pdf", 4096, false))
                using (var output = File.OpenWrite(@"\\?\C:\Work\Node.js Design Patterns - Second Edition.gz", 4096, false))
                {
                    foreach (var compressedChunk in StreamChunk.SplitStream(input, 64 * 1024)
                        .Transform(Compressor.Compress))
                    {
                        compressedChunk.Write(output);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
        }
    }
}