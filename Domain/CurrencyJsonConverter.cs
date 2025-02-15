using Newtonsoft.Json;

namespace Domain
{
    public class CurrencyJsonConverter : JsonConverter<Currency>
    {
        public override void WriteJson(JsonWriter writer, Currency value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Code);
        }

        public override Currency ReadJson(JsonReader reader, Type objectType, Currency existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var code = reader.Value as string;
            return Currency.OfCode(code);
        }
    }
}