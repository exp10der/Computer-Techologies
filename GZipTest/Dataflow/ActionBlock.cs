namespace GZipTest.Dataflow
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Queue;

    internal class ActionBlock<TSource>
    {
        private readonly BlockingQueue<TSource> _inputQueue;
        private readonly Action<Exception> _onError;
        private readonly IEnumerable<TSource> _source;
        private Exception _error;

        public ActionBlock(IEnumerable<TSource> source, int capacity, Action<Exception> onError)
        {
            _source = source;
            _onError = onError;
            _inputQueue = new BlockingQueue<TSource>(capacity);
        }

        public IEnumerable<TSource> LinkTo()
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

                        _inputQueue.Add(element);
                    }
                }
                catch (Exception e)
                {
                    if (Interlocked.CompareExchange(ref _error, e, null) == null)
                        _onError(e);
                }
                finally
                {
                    _inputQueue.CompleteAdding();
                }
            });

            task.IsBackground = true;

            task.Start();

            return _inputQueue.GetConsumingEnumerable();
        }
    }
}