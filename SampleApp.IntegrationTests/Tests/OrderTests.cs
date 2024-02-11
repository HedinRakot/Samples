using FluentAssertions;
using Xunit;

namespace SampleApp.IntegrationTests.Tests;

public class OrderTests
{
    [Fact]
    public void OrderTests_Order_Has_No_History()
    {
        var sut = new Domain.Order();
        sut.HasHistory().Should().BeFalse();
    }
}
