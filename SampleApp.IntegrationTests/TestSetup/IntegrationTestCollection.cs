using Xunit;

namespace SampleApp.IntegrationTests.TestSetup;

[CollectionDefinition(Name)]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestsFixture>
{
    public const string Name = "SampleApp";
}
