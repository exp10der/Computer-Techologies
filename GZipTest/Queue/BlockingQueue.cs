namespace GZipTest.Queue
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal class BlockingQueue<T> : IDisposable
    {
        private readonly Queue<T> _queue;
        private readonly object _queueLockObject = new object();
        private Semaphore _itemsAvailable;
        private Semaphore _spaceAvailable;

        public BlockingQueue(int size)
        {
            Initialize(size);

            _queue = new Queue<T>(size);
        }

        public bool IsClosed { get; private set; }

        void IDisposable.Dispose()
        {
            if (_itemsAvailable == null) return;
            _itemsAvailable.Close();
            _spaceAvailable.Close();
            _itemsAvailable = null;
        }

        private void Initialize(int size)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Must be greater than zero.");

            _itemsAvailable = new Semaphore(0, size);
            _spaceAvailable = new Semaphore(size, size);
        }

        public void Add(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            //Wait if the collection is full
            _spaceAvailable.WaitOne();

            lock (_queueLockObject)
            {
                if (IsClosed) throw new InvalidOperationException(nameof(IsClosed));
                _queue.Enqueue(data);
            }
            _itemsAvailable.Release();
        }

        public T Dequeue()
        {
            T item;

            if (_queue.Count == 0 && IsClosed)
                throw new InvalidOperationException(nameof(IsClosed));

            _itemsAvailable.WaitOne();

            lock (_queueLockObject)
            {
                if (_queue.Count == 0 && IsClosed)
                {
                    _itemsAvailable.Release();
                    throw new InvalidOperationException(nameof(IsClosed));
                }

                item = _queue.Dequeue();
            }

            _spaceAvailable.Release();
            return item;
        }

        public IEnumerable<T> GetConsumingEnumerable()
        {
            while (true)
            {
                T item;

                try
                {
                    item = Dequeue();
                }
                catch (Exception e) when (e is InvalidOperationException)
                {
                    break;
                }
                yield return item;
            }
        }

        protected object SyncRoot() => _queueLockObject;

        public void CompleteAdding()
        {
            if (IsClosed) return;
            lock (_queueLockObject)
            {
                IsClosed = true;

                // 55 line code, any waiting blocked threads need to get access to find out the queue is closed
                try
                {
                    _itemsAvailable.Release();
                }
                catch (SemaphoreFullException)
                {
                    // ...
                }
            }
        }
    }
}