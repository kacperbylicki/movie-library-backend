using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class RefreshTokenDto
{
    [JsonPropertyName("userId")]
    [Required]
    public string UserId { get; set; } = default!;
    
    [JsonPropertyName("refreshToken")]
    [Required]
    public string RefreshToken { get; set; } = default!;
}