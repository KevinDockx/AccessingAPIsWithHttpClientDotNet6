using Movies.Client.Helpers;
using Movies.Client.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Movies.Client;

public class MoviesAPIClient
{
    private HttpClient _client;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public MoviesAPIClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _client = client;
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ??
            throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
        _client.BaseAddress = new Uri("http://localhost:5001");
        _client.Timeout = new TimeSpan(0, 0, 30);
    }

    public async Task<IEnumerable<Movie>?> GetMoviesAsync()
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/movies");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<IEnumerable<Movie>>(content,
            _jsonSerializerOptionsWrapper.Options);
    }

}
