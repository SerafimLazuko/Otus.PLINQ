
# Суммирование элементов массива интов

В данном проекте реализованы три способа вычисления суммы элементов массива интов с использованием паттерна "Стратегия":

1. **Обычное (последовательное) вычисление** - класс `Int32OrdinaryCalculatorProvider`.
2. **Параллельное вычисление с использованием Thread** - класс `Int32ThreadCalculatorProvider`.
3. **Параллельное вычисление с использованием PLINQ** - класс `Int32PLINQCalculatorProvider`.

## Используемый паттерн

В этом проекте используется паттерн "Стратегия", который позволяет выбирать алгоритмы на основе переданного провайдера. Центральным элементом является статический класс `SequenceSumCalculator`, который вызывает метод `CalculateSum` на переданном провайдере.

```csharp
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
```

Интерфейс провайдера определяет метод `CalculateSum`, который должен быть реализован всеми конкретными провайдерами.

```csharp
namespace Otus.PLINQ.CalculatorProviders
{
    public interface ICalculatorProvider<T>
    {
        CalculationResult CalculateSum(IEnumerable<T> sequence);
    }
}
```

## Реализация конкретных провайдеров

### Обычное (последовательное) вычисление

Этот провайдер использует простой цикл `foreach` для последовательного суммирования элементов массива.

```csharp
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
```

### Параллельное вычисление с использованием Thread

Этот провайдер использует многопоточность для параллельного суммирования элементов массива. Массив делится на равные части, и каждая часть обрабатывается в отдельном потоке.

```csharp
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

            stopwatch.Start();
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
```

### Параллельное вычисление с использованием PLINQ

Этот провайдер использует PLINQ для параллельного суммирования элементов массива.

```csharp
namespace Otus.PLINQ.CalculatorProviders.Int32
{
    public class Int32PLINQCalculatorProvider : ICalculatorProvider<int>
    {
        public CalculationResult CalculateSum(IEnumerable<int> sequence)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var plinqSum = sequence.AsParallel().Sum(s => (long)s);
            stopwatch.Stop();

            return new CalculationResult { Sum = plinqSum, Count = sequence.Count(), Duration = stopwatch.Elapsed };
        }
    }
}
```

## Результаты замеров

Окружение (характеристики компьютера и ОС): ЦП

	13th Gen Intel(R) Core(TM) i5-13400
	Базовая скорость:	2,50 ГГц
	Сокетов:	1
	Ядра:	10
	Логических процессоров:	16
	Виртуализация:	Включено
	Кэш L1:	800 КБ
	Кэш L2:	8,8 МБ
	Кэш L3:	20,0 МБ


| Метод                               | 100,000 элементов | 1,000,000 элементов | 10,000,000 элементов |
|-------------------------------------|-------------------|---------------------|----------------------|
| Обычное вычисление                  | 0.35 мс           | 3.7747 мс           | 31.8577 мс           |
| Параллельное вычисление (Threads)   | 58.2179 мс        | 72.618 мс           | 123.4572 мс          |
| Параллельное вычисление (PLINQ)     | 57.2086 мс        | 4.83 мс             | 51.882 мс            |

## Пример использования

Для тестирования и демонстрации работы алгоритмов был использован следующий код:

```csharp
var sequnce100k = Enumerable.Range(0, 100000);
var sequnce1Bill = Enumerable.Range(0, 1000000);
var sequnce10Bill = Enumerable.Range(0, 10000000);

// Обычное вычисление суммы элементов массива интов
Console.WriteLine($"Обычное вычисление суммы элементов массива интов.");

var ordinaryProvider = new Int32OrdinaryCalculatorProvider();

var ordinalCalculation100kResult = SequenceSumCalculator<int>.CalculateSum(sequnce100k, ordinaryProvider);
var ordinalCalculation1BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce1Bill, ordinaryProvider);
var ordinalCalculation10BillResult = SequenceSumCalculator<int>.CalculateSum(sequnce10Bill, ordinaryProvider);

Console.WriteLine(ordinalCalculation100kResult.CalculationResultMessage);
Console.WriteLine(ordinalCalculation1BillResult.CalculationResultMessage);
Console.WriteLine(ordinalCalculation10BillResult.CalculationResultMessage);

// Параллельное вычисление суммы элементов массива интов (для реализации использовать Thread, например List)
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

// Параллельное вычисление с помощью LINQ суммы элементов массива интов
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
```

## Выводы

В этом проекте были продемонстрированы различные подходы к суммированию элементов массива интов: последовательный, многопоточный с использованием Thread, и параллельный с использованием PLINQ. Результаты показывают, что последовательный метод быстрее для небольших объемов данных, в то время как PLINQ обеспечивает лучшие результаты на больших объемах данных.

