using SampleApi.Application.Commands;

namespace SampleApi.Application;

internal class TestCommandHandler : ITestCommandHandler
{
    public Task Handle(TestDomainCommand domainCommand)
    {
        //fachliche Logik
        //z.B in DB speichern

        return Task.CompletedTask;
    }
}
