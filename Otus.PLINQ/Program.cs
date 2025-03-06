using Otus.PLINQ;
using Otus.PLINQ.CalculatorProviders.Int32;

var sequnce100k = Enumerable.Range(0, 100000);
var sequnce1Bill = Enumerable.Range(0, 1000000);
var sequnce10Bill = Enumerable.Range(0, 10000000);

//Обычное вычисление суммы элементов массива интов
Console.WriteLine($"Обычное вычисление суммы элементов массива интов.");

var ordinaryProvider = new Int32OrdinaryCalculatorProvider();

var ordinalCalculation100kResult = SequenceSumCalculator<int>.CalculateSum(sequnce100k, ordinaryProvider);
var ordinalCalculation1BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce1Bill, ordinaryProvider);
var ordinalCalculation10BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce10Bill, ordinaryProvider);

Console.WriteLine(ordinalCalculation100kResult.CalculationResultMessage);
Console.WriteLine(ordinalCalculation1BillResult.CalculationResultMessage);
Console.WriteLine(ordinalCalculation10BillResult.CalculationResultMessage);

//Параллельное вычисление суммы элементов массива интов (для реализации использовать Thread, например List)
Console.WriteLine();
Console.WriteLine("Параллельное вычисление суммы с помощью List<Thread>");

var threadsCount = 16;
var threadsProvider = new Int32ThreadCalculatorProvider(threadsCount);

var threadsCalculation100kResult = SequenceSumCalculator<int>.CalculateSum(sequnce100k, threadsProvider);
var threadsCalculation1BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce1Bill, threadsProvider);
var threadsCalculation10BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce10Bill, threadsProvider);

Console.WriteLine(threadsCalculation100kResult.CalculationResultMessage);
Console.WriteLine(threadsCalculation1BillResult.CalculationResultMessage);
Console.WriteLine(threadsCalculation10BillResult.CalculationResultMessage);

//Параллельное вычисление с помощью LINQ суммы элементов массива интов
Console.WriteLine();
Console.WriteLine("Параллельное вычисление суммы с помощью LINQ");

var plinqProvider = new Int32PLINQCalculatorProvider();
    
var plinqCalculation100kResult = SequenceSumCalculator<int>.CalculateSum(sequnce100k, plinqProvider);
var plinqCalculation1BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce1Bill, plinqProvider);
var plinqCalculation10BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce10Bill, plinqProvider);

Console.WriteLine(plinqCalculation100kResult.CalculationResultMessage);
Console.WriteLine(plinqCalculation1BillResult.CalculationResultMessage);
Console.WriteLine(plinqCalculation10BillResult.CalculationResultMessage);

Console.ReadKey();