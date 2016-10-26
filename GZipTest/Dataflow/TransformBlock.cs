namespace GZipTest.Dataflow
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Queue;

    internal class TransformBlock<TInput, TOutput>
    {
        // Get the number of processors, initialize the number of remaining
        // threads, and set the starting point for the iteration.
        private readonly int _numProcs;
        private readonly Action<Exception> _onError;
        private readonly BlockingQueue<TOutput> _output;
        private readonly IEnumerable<TInput> _source;
        private Exception _error;

        public TransformBlock(IEnumerable<TInput> source, int capacity, Action<Exception> onError)
        {
            _source = source;
            _output = new BlockingQueue<TOutput>(capacity);
            _onError = onError;
            _numProcs = Environment.ProcessorCount;
        }

        public IEnumerable<TOutput> Consume(Func<TInput, TOutput> compressSelector)
        {
            int remainingWorkItems = _numProcs;

            // Create each of the work items.
            for (var p = 0; p < _numProcs; p++)
            {
                var task = new Thread(() =>
                {
                    try
                    {
                        foreach (var element in _source)
                        {
                            //http://www.albahari.com/threading/part4.aspx
                            Thread.MemoryBarrier();
                            if (_error != null) break;


                            var copressed = compressSelector(element);
                            _output.Add(copressed);
                        }
                    }
                    catch (Exception e)
                    {
                        if (Interlocked.CompareExchange(ref _error, e, null) == null)
                            _onError(e);
                    }
                    finally
                    {
                        if (Interlocked.Decrement(ref remainingWorkItems) == 0)
                            _output.CompleteAdding();
                    }
                });

                task.IsBackground = true;

                task.Start();
            }


            return _output.GetConsumingEnumerable();
        }
    }
}