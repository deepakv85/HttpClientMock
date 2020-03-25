using System.Text.Json.Serialization;

namespace MovieRating.Domain
{
    public class Movie
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Year")]
        public int Year { get; set; }

        [JsonPropertyName("imdbRating")]
        public decimal imdbRating { get; set; }

        [JsonPropertyName("imdbID")]
        public string imdbID { get; set; }
    }
}
