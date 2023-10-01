using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class RegisteredUserResponseDto
{
    [JsonPropertyName("userId")]
    [Required]
    public string UserId { get; set; } = default!;
    
    [JsonPropertyName("userConfirmed")]
    [Required]
    public bool UserConfirmed { get; set; } = default!;
    
    [JsonPropertyName("destination")]
    [Required]
    public string Destination { get; set; } = default!;
}