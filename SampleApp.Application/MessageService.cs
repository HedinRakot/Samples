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
            await _messageContext.Publish(new TestEvent
            {
                Count = 100
            });

            await _messageContext.Send(new TestCommand
            {
                Count = 100
            });
        }
        catch (Exception ex)
        {
            throw new DomainException($"Es ist ein unerwarteter Fehler beim SampleApi Service Aufruf aufgetreten.", ex);
        }
    }
}
