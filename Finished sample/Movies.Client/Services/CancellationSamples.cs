using Movies.Client.Helpers;
using Movies.Client.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Movies.Client.Services;

public class CancellationSamples : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private CancellationTokenSource _cancellationTokenSource =
        new CancellationTokenSource();

    public CancellationSamples(IHttpClientFactory httpClientFactory,
             JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ??
            throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
    }


    public async Task RunAsync()
    {
        _cancellationTokenSource.CancelAfter(200);
        // await GetTrailerAndCancelAsync(_cancellationTokenSource.Token);
        await GetTrailerAndHandleTimeoutAsync();
    }

    private async Task GetTrailerAndCancelAsync(CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/trailers/{Guid.NewGuid()}");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(
            new StringWithQualityHeaderValue("gzip"));

        try
        {
            using (var response = await httpClient.SendAsync(request,
             HttpCompletionOption.ResponseHeadersRead, cancellationToken))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                response.EnsureSuccessStatusCode();

                var poster = await JsonSerializer.DeserializeAsync<Trailer>(
                    stream,
                    _jsonSerializerOptionsWrapper.Options);
            }
        }
        catch (OperationCanceledException ocException)
        {
            Console.WriteLine($"An operation was cancelled with message {ocException.Message}.");
            // additional cleanup, ...
        }
    }

    private async Task GetTrailerAndHandleTimeoutAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/trailers/{Guid.NewGuid()}");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(
            new StringWithQualityHeaderValue("gzip"));

        try
        {
            using (var response = await httpClient.SendAsync(request,
             HttpCompletionOption.ResponseHeadersRead))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                response.EnsureSuccessStatusCode();

                var poster = await JsonSerializer.DeserializeAsync<Trailer>(
                    stream,
                    _jsonSerializerOptionsWrapper.Options);
            }
        }
        catch (OperationCanceledException ocException)
        {
            Console.WriteLine($"An operation was cancelled with message {ocException.Message}.");
            // additional cleanup, ...
        }
    }

}
