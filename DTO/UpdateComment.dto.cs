using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class UpdateCommentDto
{
    [JsonPropertyName("content")]
    [Required, MinLength(2)]
    public string Content { get; set; } = default!; 
}