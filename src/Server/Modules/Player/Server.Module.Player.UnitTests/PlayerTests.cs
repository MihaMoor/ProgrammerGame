using Server.Module.Player.Domain;
using Server.Shared.Errors;
using Server.Shared.Results;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Server.Module.Player.UnitTests;

public partial class PlayerTests
{
    [TimeoutTheory(10)]
    [MemberData(nameof(CreateMainStatsData))]
    public void Cоздания_основных_характеристик(string name, bool expected, Error? error)
    {
        Result<MainStats> actual = MainStats.CreatePlayer(name);

        Assert.Equal(expected, actual.IsSuccess);
        if (actual.IsFailure)
        {
            Assert.Equal(error, actual.Error);
        }
    }
}

// Атрибут для использования в тестах
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
[XunitTestCaseDiscoverer(
    "Server.Module.Player.UnitTests.TimeoutTestCaseDiscoverer",
    "Server.Module.Player.UnitTests"
)]
public class TimeoutTheoryAttribute(int timeoutMilliseconds) : TheoryAttribute
{
    public int TimeoutMilliseconds { get; } = timeoutMilliseconds;
}

public class TimeoutTestCaseDiscoverer : IXunitTestCaseDiscoverer
{
    public IEnumerable<IXunitTestCase> Discover(
        ITestMethod testMethod,
        IAttributeInfo factAttribute
    )
    {
        int timeout = factAttribute.GetNamedArgument<int>("TimeoutMilliseconds");
        yield return new TimeoutTestCase(testMethod, timeout);
    }

    public IEnumerable<IXunitTestCase> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo factAttribute
    ) => Discover(testMethod, factAttribute);
}

public class TimeoutTestCase : LongLivedMarshalByRefObject, IXunitTestCase
{
    [Obsolete(
        "Called by the de-serializer; should only be called by deriving classes for de-serialization purposes"
    )]
    public TimeoutTestCase() { }

    public IMethodInfo Method { get; }

    public int TimeoutMilliseconds { get; }

    public string DisplayName => $"{Method.Name} (Timeout {TimeoutMilliseconds} ms)";

    public Exception InitializationException => throw new NotImplementedException();

    public int Timeout => throw new NotImplementedException();

    public string SkipReason => throw new NotImplementedException();

    public ISourceInformation SourceInformation
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public ITestMethod TestMethod => throw new NotImplementedException();

    public object[] TestMethodArguments => throw new NotImplementedException();

    public Dictionary<string, List<string>> Traits => throw new NotImplementedException();

    public string UniqueID => throw new NotImplementedException();

    // Конструктор для десериализации
    public TimeoutTestCase(ITestMethod testMethod, int timeoutMilliseconds)
    {
        Method = testMethod.Method;
        TimeoutMilliseconds = timeoutMilliseconds;
    }

    // Метод для запуска теста
    public async Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageSink executionMessageSink,
        ITestMethod testMethod,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource
    )
    {
        RunSummary runSummary = new RunSummary();

        // Создаем отдельный таск для выполнения теста
        //var testTask = testMethod.RunAsync(
        //    diagnosticMessageSink,
        //    executionMessageSink,
        //    constructorArguments,
        //    aggregator,
        //    cancellationTokenSource
        //);
        MessageBus messageBus = new MessageBus(diagnosticMessageSink);
        XunitTestMethodRunner runner = new XunitTestMethodRunner(
            testMethod, // ITestMethod
            testMethod.TestClass.Class as IReflectionTypeInfo, // IReflectionTypeInfo
            testMethod.Method as IReflectionMethodInfo, // IReflectionMethodInfo
            [this], // IEnumerable<IXunitTestCase>
            diagnosticMessageSink, // IMessageSink
            messageBus, // IMessageBus — обязательно!
            aggregator, // ExceptionAggregator
            cancellationTokenSource, // CancellationTokenSource
            constructorArguments // object[]
        );

        Task<RunSummary> testTask = runner.RunAsync();

        // Создаем токен для тайм-аута
        CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationTokenSource.Token
        );
        Task delayTask = Task.Delay(TimeoutMilliseconds, cts.Token);

        // Ждем либо завершения теста, либо срабатывания тайм-аута
        if (await Task.WhenAny(testTask, delayTask) == testTask)
        {
            // Тест завершился первым
            cts.Cancel(); // отменяем задержку
            RunSummary result = await testTask;
            return result;
        }
        else
        {
            // Тайм-аут сработал
            cts.Cancel(); // отменяем выполнение теста
            string message = $"Тест превысил лимит времени {TimeoutMilliseconds} мс.";
            // Можно выбросить исключение или записать в отчет
            throw new XunitException(message);
        }
    }

    // Реализуем сериализацию
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(Method), Method.ToString());
        info.AddValue(nameof(TimeoutMilliseconds), TimeoutMilliseconds);
    }

    // Десериализация
    public static IXunitTestCase Deserialize(IXunitSerializationInfo info)
    {
        string methodDisplayName = info.GetValue<string>(nameof(Method));
        int timeout = info.GetValue<int>(nameof(TimeoutMilliseconds));
        // Здесь нужно восстановить IMethodInfo по имени
        // Для этого потребуется доступ к ITestMethod или к TestAssembly, что усложняет
        // Поэтому обычно используют фабрику или регистрируют через Discoverer
        throw new NotImplementedException(
            "Реализация десериализации требует доступа к IMethodInfo"
        );
    }

    public Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource
    )
    {
        throw new NotImplementedException();
    }

    void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
    {
        throw new NotImplementedException();
    }
}
