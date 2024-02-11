using FluentAssertions;
using NSubstitute;
using SampleApp.IntegrationTests.TestSetup;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SampleApp.IntegrationTests.Tests;

[Collection(IntegrationTestCollection.Name)]
public class ApiTestControllerTests //: IClassFixture<IntegrationTestsFixture>
{
    private const string RequestUri = "ApiTest";
    private readonly HttpClient _httpClient;
    private readonly IntegrationTestsFixture _fixture;

    public ApiTestControllerTests(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
        _httpClient = _fixture.CreateClient();

        _httpClient.DefaultRequestHeaders.Add("x-api-key", "1234567890");
    }

    [Fact]
    public async Task ApiTest_Index_Returns_Result()
    {
        //arrange
        _fixture.TestOrderService.GetOrders().Returns(
            new List<Domain.Order>()
            {
                new Domain.Order
                {
                    Customer = new Domain.Customer
                    {
                        Email = "testemail@test.de"
                    }
                }
            });

        //act
        var response = await _httpClient.GetAsync(RequestUri);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Domain.Order>>();

        result.Count.Should().BeGreaterThan(0);
        result.FirstOrDefault().Customer.Should().NotBeNull();
        result.FirstOrDefault().Customer.Email.Should().Be("testemail@test.de");
    }

    [Theory]
    [InlineData("1", "testemail@test.de")]
    [InlineData("2", "testemail2@test.de")]
    public async Task ApiTest_Index_Returns_Customer_With_Data_Result(string orderNumber, string email)
    {
        //arrange
        _fixture.TestOrderService.GetOrders().Returns(
            new List<Domain.Order>()
            {
                new Domain.Order
                {
                    OrderNumber = orderNumber,
                    Customer = new Domain.Customer
                    {
                        Email = email,
                    }
                }
            });

        //act
        var response = await _httpClient.GetAsync(RequestUri);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Domain.Order>>();

        result.Should().NotBeNull();
        result.Count.Should().BeGreaterThan(0);
        result.FirstOrDefault().OrderNumber.Should().Be(orderNumber);
        result.FirstOrDefault().Customer.Should().NotBeNull();
        result.FirstOrDefault().Customer.Email.Should().Be(email);
    }


    [Fact]
    public async Task ApiTest_Add_Order_Returns_Result()
    {
        //arrange
        var newOrder = new Domain.Order
        {
            OrderNumber = "1"
        };

        //act
        var response = await _httpClient.PostAsync(RequestUri + "AddOrder",
            new StringContent(JsonSerializer.Serialize(newOrder), Encoding.UTF8, "application/json"));

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<Domain.Order>();
        //TODO response testen


        //db tests
        var dbOrder = _fixture.GetOrder("1");
        dbOrder.Should().NotBeNull();
    }
}
