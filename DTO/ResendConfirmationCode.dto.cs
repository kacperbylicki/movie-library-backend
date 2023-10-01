using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class ResendConfirmationCodeDto
{
    [JsonPropertyName("email")]
    [Required, EmailAddress()]
    public string Email { get; set; } = default!;
}