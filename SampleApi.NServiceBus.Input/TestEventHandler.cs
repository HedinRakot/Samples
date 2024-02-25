using NServiceBus;

namespace SampleApi.NServiceBus.Input;

using SampleApi.Application;
using SampleApp.Messages;

public class TestEventHandler : IHandleMessages<TestEvent>
{
    public Task Handle(TestEvent testEvent, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}