using System.Diagnostics;

namespace Otus.PLINQ.CalculatorProviders.Int32
{
    public class Int32ThreadCalculatorProvider : ICalculatorProvider<int>
    {
        public int ThreadsCount { get; }

        public Int32ThreadCalculatorProvider(int threadsCount)
        {
            ThreadsCount = threadsCount;
        }

        public CalculationResult CalculateSum(IEnumerable<int> sequence)
        {
            var stopwatch = new Stopwatch();
            var threads = new List<Thread>();
            long threadsSum = 0;

            for (int i = 0; i < ThreadsCount; i++)
            {
                var processingDataPerThread = sequence.Skip(i * sequence.Count() / ThreadsCount).Take(sequence.Count() / ThreadsCount);

                var thread = new Thread(() =>
                {
                    foreach (var el in processingDataPerThread)
                        Interlocked.Add(ref threadsSum, el);
                });

                threads.Add(thread);
            }

            stopwatch.Restart();
            foreach (var thread in threads)
            {
                thread.Start();
                thread.Join();
            }
            stopwatch.Stop();

            return new CalculationResult { Sum = threadsSum, Count = sequence.Count(), Duration = stopwatch.Elapsed };
        }
    }
}
