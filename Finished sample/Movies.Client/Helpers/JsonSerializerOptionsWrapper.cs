using System.Text.Json;

namespace Movies.Client.Helpers;

public class JsonSerializerOptionsWrapper
{
    public JsonSerializerOptions Options { get; }

    public JsonSerializerOptionsWrapper()
    {
        Options = new JsonSerializerOptions(
            JsonSerializerDefaults.Web);
        Options.DefaultBufferSize = 10;
    }
}
