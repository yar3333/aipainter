using System.Text.Json.Serialization;
using System.Text.Json;

namespace AiPainter.Helpers;

// JsonElement to create an immutable object or JsonNode to create a mutable object

class JsonForLogConverter : JsonConverter<JsonElement>
{
    public override JsonElement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException();
    }

    public override void Write(Utf8JsonWriter writer, JsonElement value, JsonSerializerOptions options)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                writer.WriteStartObject();
                foreach (var v in value.EnumerateObject())
                {
                    writer.WritePropertyName(v.Name);
                    JsonSerializer.Serialize(writer, v.Value, options);
                }
                writer.WriteEndObject();
                break;

            case JsonValueKind.Array:
                writer.WriteStartArray();
                foreach (var v in value.EnumerateArray())
                {
                    JsonSerializer.Serialize(writer, v, options);
                }
                writer.WriteEndArray();
                break;

            case JsonValueKind.String:
                var text = value.GetString();
                if (text == null) writer.WriteStringValue((string?)null);
                else writer.WriteStringValue(text.Length <= 100 ? text : text[..98] + "..");
                break;

            default:
                JsonSerializer.Serialize(writer, value);
                break;
        }
    }

    public static string Serialize(JsonElement obj)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = null,
            Converters = { new JsonForLogConverter() },
            Encoder = Program.DefaultJsonSerializerOptions.Encoder,
        };
        return JsonSerializer.Serialize(obj, options)!;
    }
}
