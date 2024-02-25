using NServiceBus;
using SampleApi.Application;
using SampleApi.Application.Commands;
using SampleApp.Messages;

namespace SampleApi.NServiceBus.Input;

public class TestFromAppCommandHandler : IHandleMessages<TestCommand>
{
    private readonly ITestCommandHandler _testCommandHandler;

    public TestFromAppCommandHandler(ITestCommandHandler testCommandHandler)
    {
        _testCommandHandler = testCommandHandler;
    }

    public async Task Handle(TestCommand testCommand, IMessageHandlerContext context)
    {
        var domain = new TestDomainCommand()
        {
            Count = testCommand.Count,
        };

        await _testCommandHandler.Handle(domain);
    }
}