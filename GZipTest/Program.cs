namespace GZipTest
{
    using System;
    using System.Threading;
    using Queue;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var countIter = 40;
            var remainingWorkThread = countIter;

            using (var context = new ThreadQueue(8))
            {
                using (var mre = new ManualResetEvent(false))
                {
                    for (var i = 0; i < countIter; i++)
                    {
                        var index = i;

                        context.Enqueue(() => { Console.WriteLine(index); });

                        if (Interlocked.Decrement(ref remainingWorkThread) == 0)
                            mre.Set();
                    }

                    // Wait for all threads to complete.
                    mre.WaitOne();
                }
            }
        }
    }
}