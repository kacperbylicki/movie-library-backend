using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class ResendConfirmationCodeResponseDto
{
    [JsonPropertyName("destination")]
    [Required]
    public string Destination { get; set; } = default!;
}