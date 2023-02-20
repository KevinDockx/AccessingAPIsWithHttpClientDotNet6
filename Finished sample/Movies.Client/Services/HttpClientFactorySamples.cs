using Movies.Client.Helpers;
using Movies.Client.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Movies.Client.Services;

public class HttpClientFactorySamples : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly MoviesAPIClient _moviesAPIClient;

    public HttpClientFactorySamples(IHttpClientFactory httpClientFactory,
             JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
             MoviesAPIClient moviesAPIClient)
    {
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ??
            throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
        _moviesAPIClient = moviesAPIClient ?? 
            throw new ArgumentNullException(nameof(moviesAPIClient));
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task RunAsync()
    {
        // await TestDisposeHttpClientAsync();
        // await TestReuseHttpClientAsync();
        // await GetFilmsAsync();
        // await GetMoviesWithTypedHttpClientAsync();
        await GetMoviesViaMoviesAPIClientAsync();
    }

    public async Task GetFilmsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/films");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(
            content,
            _jsonSerializerOptionsWrapper.Options);
    }

    private async Task GetMoviesViaMoviesAPIClientAsync()
    {
        var movies = await _moviesAPIClient.GetMoviesAsync();
    }


    //private async Task GetMoviesWithTypedHttpClientAsync()
    //{
    //    var request = new HttpRequestMessage(
    //        HttpMethod.Get,
    //        "api/movies");
    //    request.Headers.Accept.Add(
    //        new MediaTypeWithQualityHeaderValue("application/json"));

    //    var response = await _moviesAPIClient.Client.SendAsync(request);
    //    response.EnsureSuccessStatusCode();

    //    var content = await response.Content.ReadAsStringAsync();

    //    var movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(content,
    //        _jsonSerializerOptionsWrapper.Options);
    //}


    private async Task TestDisposeHttpClientAsync()
    {
        for (var i = 0; i < 10; i++)
        {
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    "https://www.google.com");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Request completed with status code " +
                    $"{response.StatusCode}");
            }
        }
    }

    private async Task TestReuseHttpClientAsync()
    {
        var httpClient = new HttpClient();

        for (int i = 0; i < 10; i++)
        {
            var request = new HttpRequestMessage(
            HttpMethod.Get,
            "https://www.google.com");

            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Request completed with status code " +
                $"{response.StatusCode}");
        }
    }


}
