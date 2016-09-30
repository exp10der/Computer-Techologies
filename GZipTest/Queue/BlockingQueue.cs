namespace GZipTest.Queue
{
    using System;
    using System.Collections.Generic;

    internal class BlockingQueue<T> : IDisposable
    {
        private readonly Queue<T> _queue;
        private readonly object _queueLockObject = new object();

        public BlockingQueue(int size)
        {
            Initialize();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Add(T data)
        {
            throw new NotImplementedException();
        }

        public T Dequeue()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetConsumingEnumerable()
        {
            throw new NotImplementedException();
        }

        public object SyncRoot() => _queueLockObject;

        public void CompleteAdding()
        {
            throw new NotImplementedException();
        }
    }
}