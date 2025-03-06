using Otus.PLINQ.CalculatorProviders;
using System.Diagnostics;

namespace Otus.PLINQ.CalculatorProviders.Int32
{
    public class Int32PLINQCalculatorProvider : ICalculatorProvider<int>
    {
        public CalculationResult CalculateSum(IEnumerable<int> sequence)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Restart();
            var plinqSum = sequence.AsParallel().Sum(s => (long)s);
            stopwatch.Stop();

            return new CalculationResult { Sum = plinqSum, Count = sequence.Count(), Duration = stopwatch.Elapsed };
        }
    }
}
