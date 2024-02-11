using FluentAssertions;
using SampleApp.IntegrationTests.TestSetup;
using System.Net;
using System.Net.Http.Json;
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
        //...

        //act
        var response = await _httpClient.GetAsync(RequestUri);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<List<Domain.Order>>();

    }
}
