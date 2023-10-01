using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class RegisterDto
{
    [JsonPropertyName("email")]
    [Required, EmailAddress()]
    public string Email { get; set; } = default!;
    
    [JsonPropertyName("username")]
    [Required, MinLength(2)]
    public string Username { get; set; } = default!;
    
    [JsonPropertyName("password")]
    [Required, MinLength(8)]
    public string Password { get; set; } = default!;
    
    [JsonPropertyName("passwordConfirmation")]
    [Required, MinLength(8), Compare("Password")]
    public string PasswordConfirmation { get; set; } = default!;
}