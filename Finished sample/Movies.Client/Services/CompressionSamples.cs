using Movies.Client.Helpers;
using Movies.Client.Models;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Movies.Client.Services;

public class CompressionSamples : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public CompressionSamples(IHttpClientFactory httpClientFactory,
             JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ??
            throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
    }
    public async Task RunAsync()
    {
        // await GetPosterWithGZipCompressionAsync();
        await SendAndReceivePosterWithGZipCompressionAsync();
    }

    private async Task GetPosterWithGZipCompressionAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters/{Guid.NewGuid()}");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(
            new StringWithQualityHeaderValue("gzip"));

        using (var response = await httpClient.SendAsync(request,
           HttpCompletionOption.ResponseHeadersRead))
        {
            var stream = await response.Content.ReadAsStreamAsync();

            response.EnsureSuccessStatusCode();

            var poster = await JsonSerializer.DeserializeAsync<Poster>(
                stream,
                _jsonSerializerOptionsWrapper.Options);
        }
    }

    private async Task SendAndReceivePosterWithGZipCompressionAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        // generate a movie poster of 5MB
        var random = new Random();
        var generatedBytes = new byte[5242880];
        random.NextBytes(generatedBytes);

        var posterForCreation = new PosterForCreation()
        {
            Name = "A new poster for The Big Lebowski",
            Bytes = generatedBytes
        };

        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                posterForCreation);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters"))
            {
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(
                  new StringWithQualityHeaderValue("gzip"));

                using (var compressedMemoryContentStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(
                            compressedMemoryContentStream,
                            CompressionMode.Compress))
                    {
                        memoryContentStream.CopyTo(gzipStream);
                        gzipStream.Flush();
                        compressedMemoryContentStream.Position = 0;

                        using (var streamContent = new StreamContent(compressedMemoryContentStream))
                        {
                            streamContent.Headers.ContentType =
                                new MediaTypeHeaderValue("application/json");
                            streamContent.Headers.ContentEncoding.Add("gzip");

                            request.Content = streamContent;

                            var response = await httpClient.SendAsync(request,
                                HttpCompletionOption.ResponseHeadersRead);
                            response.EnsureSuccessStatusCode();

                            var stream = await response.Content.ReadAsStreamAsync();
                            var poster = await JsonSerializer.DeserializeAsync<Poster>(
                                stream,
                                _jsonSerializerOptionsWrapper.Options);

                            // do something with the newly created poster     

                        }
                    }
                }
            }
        }
    }
}

