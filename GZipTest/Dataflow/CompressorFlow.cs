namespace GZipTest.Dataflow
{
    using System;
    using System.Collections.Generic;
    using Models;

    internal static class CompressorFlow
    {
        public static IEnumerable<Chunk> Transform(this IEnumerable<Chunk> source, Func<Chunk,Chunk> compressSelector)
        {
            throw new NotImplementedException();
        }
    }
}