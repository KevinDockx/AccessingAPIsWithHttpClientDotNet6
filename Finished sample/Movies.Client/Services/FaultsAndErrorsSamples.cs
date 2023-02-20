using Microsoft.AspNetCore.Mvc;
using Movies.Client.Helpers;
using Movies.Client.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Movies.Client.Services;

public class FaultsAndErrorsSamples : IIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;

    public FaultsAndErrorsSamples(IHttpClientFactory httpClientFactory,
             JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper ??
            throw new ArgumentNullException(nameof(jsonSerializerOptionsWrapper));
        _httpClientFactory = httpClientFactory ??
            throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task RunAsync()
    {
        await GetMovieAndDealWithInvalidResponsesAsync(
                    CancellationToken.None);
        //await PostMovieAndHandleErrorsAsync(CancellationToken.None);
    }

    private async Task GetMovieAndDealWithInvalidResponsesAsync(
        CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "api/movies/030a43b0-f9a5-405a-811c-bf342524b2be");
        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(
            new StringWithQualityHeaderValue("gzip"));

        using (var response = await httpClient.SendAsync(request,
             HttpCompletionOption.ResponseHeadersRead,
             cancellationToken))
        {
            if (!response.IsSuccessStatusCode)
            {
                // inspect the status code
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // show this to the user
                    Console.WriteLine("The requested movie cannot be found.");
                    return;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // trigger a login flow
                    return;
                }
                response.EnsureSuccessStatusCode();
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var movie = await JsonSerializer.DeserializeAsync<Movie>(
                stream,
                _jsonSerializerOptionsWrapper.Options);

        }
    }

    private async Task PostMovieAndHandleErrorsAsync(
            CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("MoviesAPIClient");

        var movieForCreation = new MovieForCreation();

        var serializedMovieForCreation = JsonSerializer.Serialize(
                   movieForCreation,
                   _jsonSerializerOptionsWrapper.Options);

        using (var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/movies"))
        {
            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(
                new StringWithQualityHeaderValue("gzip"));
            request.Content = new StringContent(serializedMovieForCreation);
            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json");

            using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    // inspect the status code
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // read out the response body and log it to the console window
                        var errorStream = await response.Content.ReadAsStreamAsync();

                        var errorAsProblemDetails = await JsonSerializer.DeserializeAsync<ValidationProblemDetails>(
                        errorStream, 
                        _jsonSerializerOptionsWrapper.Options);

                        var errors = errorAsProblemDetails?.Errors;
                        Console.WriteLine(errorAsProblemDetails?.Title);

                        return;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // trigger a login flow
                        return;
                    }
                    response.EnsureSuccessStatusCode();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var movie = await JsonSerializer.DeserializeAsync<Movie>(
                    stream,
                    _jsonSerializerOptionsWrapper.Options);
            }
        }
    }


}