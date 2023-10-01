using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.Models;

public class Tokens
{
    [JsonPropertyName("accessToken")]
    [Required]
    public string AccessToken { get; init; }
    
    [JsonPropertyName("refreshToken")]
    [Required]
    public string? RefreshToken { get; init; }
    
    [JsonPropertyName("expiration")]
    [Required]
    public DateTimeOffset Expiration { get; init; }
}