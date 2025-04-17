using System.Text.Json.Serialization;

namespace retroGames.Collections
{
    public class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("platform")]
        public string Platform { get; set; }

        [JsonPropertyName("release_year")]
        public int ReleaseYear { get; set; }

        [JsonPropertyName("genre")]
        public string Genre { get; set; }
    }
}
