using System.Text.Json;
using Microsoft.Extensions.Options;
using SampleApp.Domain;
using SampleApp.Domain.Exceptions;
using SampleApp.Domain.Settings;

namespace SampleApp.Application;

internal class OrderHttpClient : IOrderHttpClient
{
    private readonly IOptions<ApiKeyAuthenticationOptions> _options;
    private readonly HttpClient _httpClient;

    public OrderHttpClient(IOptions<ApiKeyAuthenticationOptions> options,
        HttpClient httpClient)
    {
        _options = options;
        _httpClient = httpClient;
    }

    public async Task<List<Order>> GetOrders()
    {
        try
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "/apitest/get");
            httpRequestMessage.Headers.Add("sampleapi-api-key", "1234567890!");
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                //TODO mapping (man braucht extra Klassen (DTOs) für die externe Aufrufe,
                //die man danach auf eigene (interne) Klassen mappt
                var models = JsonSerializer.Deserialize<List<Order>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return models;
            }
            else
            {
                throw new DomainException($"Es ist ein Fehler beim SampleApi Service Aufruf aufgetreten. {httpResponseMessage.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new DomainException($"Es ist ein unerwarteter Fehler beim SampleApi Service Aufruf aufgetreten.", ex);
        }
    }
}
