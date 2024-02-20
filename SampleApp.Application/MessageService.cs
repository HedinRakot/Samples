using NServiceBus;
using SampleApp.Domain.Exceptions;
using SampleApp.Messages;

namespace SampleApp.Application;

internal class MessageService : IMessageService
{
    private readonly NServiceBus.IMessageSession _messageContext;

    public MessageService(NServiceBus.IMessageSession context)
    {
        _messageContext = context;
    }

    public async Task SendOrder()
    {
        try
        {
            var count = 101;
            await _messageContext.Publish(new TestEvent
            {
                Count = count
            });

            await _messageContext.Send(new TestCommand
            {
                Count = count
            });
        }
        catch (Exception ex)
        {
            throw new DomainException($"Es ist ein unerwarteter Fehler beim Publish/Send aufgetreten.", ex);
        }
    }
}
