namespace Otus.PLINQ.CalculatorProviders
{
    public interface ICalculatorProvider<T>
    {
        CalculationResult CalculateSum(IEnumerable<T> sequence);
    }
}