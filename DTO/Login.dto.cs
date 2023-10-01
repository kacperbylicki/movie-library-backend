using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class LoginDto
{
    [JsonPropertyName("email")]
    [Required, EmailAddress()]
    public string Email { get; set; } = default!;
    
    [JsonPropertyName("password")]
    [Required, MinLength(8)]
    public string Password { get; set; } = default!;
}