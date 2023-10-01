using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using movie_library.Models;

namespace movie_library.DTO;

public class AuthenticatedUserResponseDto
{
    [JsonPropertyName("tokens")]
    [Required]
    public Tokens Tokens { get; set; } = default!;
    
    [JsonPropertyName("user")]
    [Required]
    public User User { get; set; } = default!;   
    
    [JsonPropertyName("groups")]
    [Required]
    public List<string> Groups { get; set; } = default!;   
}