using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class CreateRatingDto
{
    [JsonPropertyName("rate")]
    [Required, Range(0, 5)]
    public decimal Rate { get; set; } = default!;
}