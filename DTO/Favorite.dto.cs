using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class FavoriteDto
{
    [JsonPropertyName("isFavorite")]
    [Required]
    public bool IsFavorite { get; set; } = default!;
}