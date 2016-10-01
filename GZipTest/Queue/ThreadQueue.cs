namespace GZipTest.Queue
{
    using System;
    using System.Threading;

    internal class ThreadQueue : IDisposable
    {
        private const int _boundedCapacity = 8;
        private readonly BlockingQueue<Action> _task;

        public ThreadQueue(int workerCount)
        {
            _task = new BlockingQueue<Action>(_boundedCapacity);

            for (var i = 0; i < workerCount; i++)
            {
                new Thread(Consume).Start();
            }
        }

        public void Dispose()
        {
            _task.CompleteAdding();
        }

        public void Enqueue(Action action)
        {
            _task.Add(action);
        }

        private void Consume()
        {
            foreach (var task in _task.GetConsumingEnumerable())
            {
                task();
            }
        }
    }
}