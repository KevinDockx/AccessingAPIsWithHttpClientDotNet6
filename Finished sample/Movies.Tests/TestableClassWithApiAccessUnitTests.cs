using Movies.Client;
using Movies.Client.Handlers;

namespace Movies.Tests;

public class TestableClassWithApiAccessUnitTests
{
    [Fact]
    public async void GetMovie_On401Response_MustThrowUnauthorizedApiAccessException()
    {
        var httpClient = new HttpClient
          (new Return401UnauthorizedResponseHandler())
        {
            BaseAddress = new Uri("http://localhost:5001")
        };

        var testableClass = new TestableClassWithApiAccess(
            httpClient,
            new ());

        await Assert.ThrowsAsync<UnauthorizedApiAccessException>(
          () => testableClass.GetMovieAsync(CancellationToken.None));
    }
}