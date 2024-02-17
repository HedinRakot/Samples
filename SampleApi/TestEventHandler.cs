//using NServiceBus;
//using SampleApp.Messages;

//namespace SampleApi;

//public class TestEventHandler : IHandleMessages<TestEvent>
//{
//    public Task Handle(TestEvent testEvent, IMessageHandlerContext context)
//    {
//        return Task.CompletedTask;
//    }
//}

//public class TestCommandHandler : IHandleMessages<TestCommand>
//{
//    public Task Handle(TestCommand testCommand, IMessageHandlerContext context)
//    {
//        return Task.CompletedTask;
//    }
//}