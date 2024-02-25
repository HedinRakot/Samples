using SampleApi.Application.Commands;

namespace SampleApi.Application;

public interface ITestCommandHandler
{
    Task Handle(TestDomainCommand domainCommand);
}