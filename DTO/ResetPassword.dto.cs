using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class ChangePasswordDto
{
    [JsonPropertyName("email")]
    [Required, EmailAddress()]
    public string Email { get; set; } = default!;
    
    [JsonPropertyName("previousPassword")]
    [Required, MinLength(8)]
    public string PreviousPassword { get; set; } = default!;
    
    [JsonPropertyName("newPassword")]
    [Required, MinLength(8)]
    public string NewPassword { get; set; } = default!;
    
    [JsonPropertyName("newPasswordConfirmation")]
    [Required, MinLength(8)]
    public string NewPasswordConfirmation { get; set; } = default!;
}