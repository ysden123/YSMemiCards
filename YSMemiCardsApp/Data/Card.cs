using System.Text.Json.Serialization;

namespace YSMemiCardsApp.Data
{
    public record Card
    {
        [JsonPropertyName("word")]
        public string Word { get; set; } = string.Empty;

        [JsonPropertyName("translations")]
        public string Translations { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; init; }

        [JsonPropertyName("lastTimeUsed")]
        public DateTime? LastTimeUsed { get; set; }

        [JsonPropertyName("level")]
        public DifficultyLevel Level { get; set; } = DifficultyLevel.Hard;

        public virtual bool Equals(Card? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Word == other.Word;
        }

        public override int GetHashCode()
        {
            return Word.GetHashCode();
        }
    }
}
