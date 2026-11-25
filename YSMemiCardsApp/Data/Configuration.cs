using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace YSMemiCardsApp.Data
{
    public record Configuration
    {
        private static readonly string ThePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "YSMemiCards", "config.json");

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
        };

        [JsonPropertyName("firstDays")]
        public int FirstDays { get; set; } = 1;

        [JsonPropertyName("secondDays")]
        public int SecondDays { get; set; } = 3;

        [JsonPropertyName("thirdDays")]
        public int ThirdDays { get; set; } = 10;

        public virtual bool Equals(Configuration? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstDays == other.FirstDays && SecondDays == other.SecondDays && ThirdDays == other.ThirdDays;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstDays, SecondDays, ThirdDays);
        }

        public static Configuration? LoadFromFile()
        {
            try
            {
                return JsonSerializer.Deserialize<Configuration>(File.ReadAllText(ThePath));
            }
            catch { return null; }
        }

        public static void SaveToFile(Configuration config)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ThePath)!);
            File.WriteAllText(ThePath, JsonSerializer.Serialize(config, SerializerOptions));
        }
    }
}
