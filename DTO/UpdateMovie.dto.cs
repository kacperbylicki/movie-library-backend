using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using movie_library.Models;

namespace movie_library.DTO
{
    public class UpdateMovieDto
    {
        [JsonPropertyName("title")]
        [MinLength(3)]
        public string? Title { get; set; }
        
        [JsonPropertyName("poster")]
        public string? Poster { get; set; }
        
        [JsonPropertyName("videoStreamKey")]
        public string? VideoStreamKey { get; set; }

        [JsonPropertyName("genre")]
        [MinLength(1)]
        public List<string>? Genre { get; set; }
        
        [JsonPropertyName("producers")]
        [MinLength(1)]
        public List<string>? Producers { get; set; }
        
        [JsonPropertyName("directors")]
        [MinLength(1)]
        public List<string>? Directors { get; set; }
        
        [JsonPropertyName("roles")]
        [MinLength(1)]
        public List<Role>? Roles { get; set; }

        [JsonPropertyName("releaseYear")]
        [Range(1888, 2023)]
        public int? ReleaseYear { get; set; }
    }
}