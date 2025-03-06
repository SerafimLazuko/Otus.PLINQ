namespace Otus.PLINQ.CalculatorProviders
{
    public class CalculationResult
    {
        public long Sum { get; set; }
        public int Count {  get; set; }
        public TimeSpan Duration { get; set; }
        public string CalculationResultMessage
            => $"Кол-во элементов: {Count}, сумма: {Sum}, время выполнения {Duration.TotalMilliseconds} мс";
    }
}
