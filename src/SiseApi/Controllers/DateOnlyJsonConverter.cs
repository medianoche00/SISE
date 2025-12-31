using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiseApi.Controllers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Intenta leer como string
            var value = reader.GetString();

            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            // Primero intenta parsear el formato DateOnly estricto
            if (DateOnly.TryParse(value, out var result))
            {
                return result;
            }

            // Si falla, intenta parsear como DateTime y extraer la fecha
            if (DateTime.TryParse(value, out var dateTimeResult))
            {
                return DateOnly.FromDateTime(dateTimeResult);
            }

            throw new JsonException($"No se pudo convertir '{value}' a DateOnly.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
