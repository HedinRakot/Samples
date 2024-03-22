﻿using Microsoft.Extensions.Options;
using SampleApp.Domain;
using SampleApp.Domain.Exceptions;
using SampleApp.Domain.Settings;
using System.Text.Json;

namespace SampleApp.Application;

internal class OrderService : IOrderService
{
    private readonly IOptions<ApiKeyAuthenticationOptions> _options;

    private readonly IOrderHttpClient _httpClient;

    public OrderService(IOptions<ApiKeyAuthenticationOptions> options,
        IOrderHttpClient httpClient
        )
    {
        _options = options;
        _httpClient = httpClient;
    }

    public async Task<List<Order>> GetOrders()
    {
        return await _httpClient.GetOrders();
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
