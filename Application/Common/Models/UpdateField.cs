using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Common.Models;
/// <summary>
/// Separates omitted properties in change requests from explicit nulls
/// </summary>
[JsonConverter(typeof(UpdateFieldJsonConverterFactory))]
public readonly struct UpdateField<T>(T? value)
{

    public bool HasValue { get; } = true;
    public T? Value { get; } = value;
}

public sealed class UpdateFieldJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(UpdateField<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type valueType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(UpdateFieldJsonConverter<>).MakeGenericType(valueType);

        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private sealed class UpdateFieldJsonConverter<T> : JsonConverter<UpdateField<T>>
    {
        public override UpdateField<T> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            T? value = JsonSerializer.Deserialize<T>(ref reader, options);

            return new UpdateField<T>(value);
        }

        public override void Write(
            Utf8JsonWriter writer,
            UpdateField<T> value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}
