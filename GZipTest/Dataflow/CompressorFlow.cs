namespace GZipTest.Dataflow
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Models;

    internal static class CompressorFlow
    {
        public static IEnumerable<Chunk> Transform(this IEnumerable<Chunk> source, Func<Chunk, Chunk> compressSelector)
        {
            Exception error = null;

            Action<Exception> onError = e => Interlocked.CompareExchange(ref error, e, null);

            // https://msdn.microsoft.com/en-us/library/ff963548.aspx
            var actionBlock = new ActionBlock<Chunk>(source, 8, onError);
            var transformBlock = new TransformBlock<Chunk, Chunk>(actionBlock.LinkTo(), 12, onError);

            foreach (var compressed in transformBlock.Consume(compressSelector))
            {
                if (error != null) break;

                yield return compressed;
            }

            if (error != null)
                throw error;
        }
    }
}