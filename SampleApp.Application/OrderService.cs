using Microsoft.Extensions.Options;
using SampleApp.Domain;
using SampleApp.Domain.Exceptions;
using SampleApp.Domain.Settings;
using System.Net.Http;
using System.Text.Json;

namespace SampleApp.Application;

internal class OrderService : IOrderService
{
    private readonly IOptions<ApiKeyAuthenticationOptions> _options;

    public OrderService(IOptions<ApiKeyAuthenticationOptions> options)
    {
        _options = options;   
    }

    public async Task<List<Order>> GetOrders()
    {
        try
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5100/order/");
            httpRequestMessage.Headers.Add("sampleapi-api-key", "1234567890!");

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

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

    public async Task<Order> AddOrder(Order order)
    {
        try
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5100/order/add");
            httpRequestMessage.Headers.Add("sampleapi-api-key", "1234567890!");

            var requestContent = JsonSerializer.Serialize(order);
            httpRequestMessage.Content = new StringContent(requestContent);

            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                //TODO mapping (man braucht extra Klassen (DTOs) für die externe Aufrufe,
                //die man danach auf eigene (interne) Klassen mappt
                var result = JsonSerializer.Deserialize<Order>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
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
