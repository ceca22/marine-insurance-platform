
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Claims.Tests
{
    public class TestJsonOptions
    {
        public static JsonSerializerOptions Default => new()
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }
}
