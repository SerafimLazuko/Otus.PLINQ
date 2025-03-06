using Otus.PLINQ.CalculatorProviders;

namespace Otus.PLINQ
{
    public static class SequenceSumCalculator<T>
    {
        public static CalculationResult CalculateSum(IEnumerable<T> sequence, ICalculatorProvider<T> calculatorProvider) 
        {
            return calculatorProvider.CalculateSum(sequence);
        }
    }
}
