using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class ConfirmAccountDto
{
    [JsonPropertyName("email")]
    [Required, EmailAddress()]
    public string Email { get; set; } = default!;
    
    [JsonPropertyName("confirmationCode")]
    [Required, MinLength(6), MaxLength(6)]
    public string ConfirmationCode { get; set; } = default!;   
}