using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using movie_library.Models;

namespace movie_library.DTO
{
    public class CreateMovieDto
    {
        [JsonPropertyName("title")]
        [Required, MinLength(3)]
        public string Title { get; set; } = default!;
        
        [JsonPropertyName("poster")]
        [Required]
        public string Poster { get; set; } = default!;
        
        [JsonPropertyName("videoStreamKey")]
        [Required]
        public string VideoStreamKey { get; set; } = default!;

        [JsonPropertyName("genre")]
        [Required, MinLength(1)]
        public List<string> Genre { get; set; } = default!;
        
        [JsonPropertyName("producers")]
        [Required, MinLength(1)]
        public List<string> Producers { get; set; } = default!;
        
        [JsonPropertyName("directors")]
        [Required, MinLength(1)]
        public List<string> Directors { get; set; } = default!;
        
        [JsonPropertyName("roles")]
        [Required, MinLength(1)]
        public List<Role> Roles { get; set; } = default!;

        [JsonPropertyName("releaseYear")]
        [Range(1888, 2023)]
        [Required]
        public int ReleaseYear { get; set; } = default!;
    }
}