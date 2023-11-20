using Microsoft.Extensions.Logging;
using RhythmRivals.BLL.Interfaces;
using System.Net.Http.Json;

namespace RhythmRivals.BLL.Services;
public class HttpService: IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpService> _logger;

    public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<TResponse?> SendAsync<TRequest, TResponse>(string requestUrl, TRequest? requestData, HttpMethod method, params KeyValuePair<string, string>[] Headers)
    {

        var content = requestData is not null ? JsonContent.Create(requestData) : null;
        var message = new HttpRequestMessage { RequestUri = new Uri(requestUrl), Content = content, Method = method };
        foreach ( var header in Headers )
        {
            message.Headers.Add(header.Key, header.Value );
        }

        return await SendAsync<TResponse>(message);
    }

    public async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage message)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            _logger.LogError("Request Error: " +  errorMessage);

            return default;
        }

        var text = await response.Content.ReadAsStringAsync();
        Console.WriteLine(text);


        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}
