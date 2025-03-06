using System.Diagnostics;

namespace Otus.PLINQ.CalculatorProviders.Int32
{
    public class Int32OrdinaryCalculatorProvider : ICalculatorProvider<int>
    {
        public CalculationResult CalculateSum(IEnumerable<int> sequence)
        {
            var stopwatch = new Stopwatch();
            long ordinarySum = 0;

            stopwatch.Start();

            foreach (var item in sequence)
                ordinarySum += item;

            stopwatch.Stop();

            return new CalculationResult { Sum = ordinarySum, Count = sequence.Count(), Duration = stopwatch.Elapsed };
        }
    }
}
