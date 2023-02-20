using Movies.Client.Helpers;
using Movies.Client.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace Movies.Client.Services;

public class CRUDSamples : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public CRUDSamples(IHttpClientFactory httpClientFactory,
             JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ??
            throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
    }


    public async Task RunAsync()
    {
        // await GetResourceAsync();
        // await GetResourceThroughHttpRequestMessageAsync();
        // await CreateResourceAsync();
        // await UpdateResourceAsync();
        await DeleteResourceAsync();
    }

    public async Task GetResourceAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient"); 

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Accept.Add(
           new MediaTypeWithQualityHeaderValue(
               "application/xml",
               0.9)); 

        var response = await httpClient.GetAsync("api/movies");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var movies = new List<Movie>();

        if (response.Content.Headers.ContentType?.MediaType
          == "application/json")
        {
            movies = JsonSerializer.Deserialize<List<Movie>>(
                content,
                _jsonSerializerOptionsWrapper.Options);
        }
        else if (response.Content.Headers.ContentType?.MediaType
            == "application/xml")
        {
            var serializer = new XmlSerializer(typeof(List<Movie>));
            movies = serializer.Deserialize(
                new StringReader(content)) as List<Movie>;
        }
    }

    public async Task GetResourceThroughHttpRequestMessageAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient"); 

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/movies");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(
            content,
            _jsonSerializerOptionsWrapper.Options);
    }

    public async Task CreateResourceAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");
        var movieToCreate = new MovieForCreation()
        {
            Title = "Reservoir Dogs",
            Description = "After a simple jewelry heist goes terribly wrong, the " +
                 "surviving criminals begin to suspect that one of them is a police informant.",
            DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
            ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
            Genre = "Crime, Drama"
        };

        var serializedMovieToCreate = JsonSerializer.Serialize(
            movieToCreate,
            _jsonSerializerOptionsWrapper.Options);

        var request = new HttpRequestMessage(
           HttpMethod.Post,
           "api/movies");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = new StringContent(serializedMovieToCreate);
        request.Content.Headers.ContentType =
            new MediaTypeHeaderValue("application/json");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var createdMovie = JsonSerializer.Deserialize<Movie>(
            content,
             _jsonSerializerOptionsWrapper.Options);

    }

    public async Task UpdateResourceAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var movieToUpdate = new MovieForUpdate()
        {
            Title = "Pulp Fiction",
            Description = "The movie with Zed.",
            DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
            ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
            Genre = "Crime, Drama"
        };

        var serializedMovieToUpdate = JsonSerializer.Serialize(
            movieToUpdate,
            _jsonSerializerOptionsWrapper.Options);

        var request = new HttpRequestMessage(
           HttpMethod.Put,
           "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new StringContent(serializedMovieToUpdate);
        request.Content.Headers.ContentType =
            new MediaTypeHeaderValue("application/json");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var updatedMovie = JsonSerializer.Deserialize<Movie>(
            content,
            _jsonSerializerOptionsWrapper.Options);
    }

    public async Task DeleteResourceAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Delete,
            "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
    }

    private async Task PostResourceShortcutAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var movieToCreate = new MovieForCreation()
        {
            Title = "Reservoir Dogs",
            Description = "After a simple jewelry heist goes terribly wrong, the " +
            "surviving criminals begin to suspect that one of them is a police informant.",
            DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
            ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
            Genre = "Crime, Drama"
        };

        var response = await httpClient.PostAsync(
            "api/movies",
            new StringContent(
                JsonSerializer.Serialize(movieToCreate,
                _jsonSerializerOptionsWrapper.Options),
                Encoding.UTF8,
                "application/json"));

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var createdMovie = JsonSerializer.Deserialize<Movie>(
            content,
            _jsonSerializerOptionsWrapper.Options);
    }

    private async Task PutResourceShortcut()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var movieToUpdate = new MovieForUpdate()
        {
            Title = "Pulp Fiction",
            Description = "The movie with Zed.",
            DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
            ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
            Genre = "Crime, Drama"
        };

        var response = await httpClient.PutAsync(
           "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
           new StringContent(
               JsonSerializer.Serialize(movieToUpdate,
                _jsonSerializerOptionsWrapper.Options),
               System.Text.Encoding.UTF8,
               "application/json"));

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var updatedMovie = JsonSerializer.Deserialize<Movie>(
            content,
            _jsonSerializerOptionsWrapper.Options);
    }

    private async Task DeleteResourceShortcut()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var response = await httpClient.DeleteAsync(
            "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
    } 
}
