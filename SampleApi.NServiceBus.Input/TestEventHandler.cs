using NServiceBus;

namespace SampleApi.NServiceBus.Input;

using SampleApp.Messages;

public class TestEventHandler : IHandleMessages<TestEvent>
{
    public Task Handle(TestEvent testEvent, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}

public class TestCommandHandler : IHandleMessages<TestCommand>
{
    public Task Handle(TestCommand testCommand, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}