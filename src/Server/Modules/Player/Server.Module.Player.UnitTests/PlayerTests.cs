using Server.Module.Player.Domain;
using Server.Shared.Errors;
using Server.Shared.Results;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Server.Module.Player.UnitTests;

public partial class PlayerTests
{
    [Theory(Timeout = 10)]
    [MemberData(nameof(CreateMainStatsData))]
    public void Cоздания_основных_характеристик(string? name, bool expected, Error? error)
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
    [Obsolete]
    public IEnumerable<IXunitTestCase> Discover(
        ITestMethod testMethod,
        IAttributeInfo factAttribute
    )
    {
        int timeout = factAttribute.GetNamedArgument<int>("TimeoutMilliseconds");
        yield return new TimeoutTestCase(testMethod, timeout);
    }

    [Obsolete]
    public IEnumerable<IXunitTestCase> Discover(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo factAttribute
    ) => Discover(testMethod, factAttribute);
}

public class TimeoutTestCase : XunitTheoryTestCase
{
    [Obsolete(
        "Called by the de-serializer; should only be called by deriving classes for de-serialization purposes"
    )]
    public TimeoutTestCase() { }

    [Obsolete]
    public TimeoutTestCase(ITestMethod testMethod, int timeoutMilliseconds)
        : base(
            diagnosticMessageSink: new NullMessageSink(),
            testMethod: testMethod,
            defaultMethodDisplay: TestMethodDisplay.Method
        ) => TimeoutMilliseconds = timeoutMilliseconds;

    public int TimeoutMilliseconds { get; }

    public override async Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource
    )
    {
        using CancellationTokenSource timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationTokenSource.Token
        );

        timeoutCts.CancelAfter(TimeoutMilliseconds);

        return await base.RunAsync(
            diagnosticMessageSink,
            messageBus,
            constructorArguments,
            aggregator,
            timeoutCts
        );
    }
}
