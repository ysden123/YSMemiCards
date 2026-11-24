using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace YSMemiCardsApp.Data
{
    public record Cards
    {
        private static readonly string ThePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YSMemiCards", "cards.json");

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };

        [JsonPropertyName("cardList")]
        public List<Card> CardsList { get; init; } = [];

        public static Cards? LoadFromFile()
        {
            try
            {
                return JsonSerializer.Deserialize<Cards>(File.ReadAllText(ThePath));
            }
            catch { return null; }
        }

        public void SaveToFile()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ThePath)!);
            File.WriteAllText(ThePath, JsonSerializer.Serialize(this, SerializerOptions));
        }
    }
}
