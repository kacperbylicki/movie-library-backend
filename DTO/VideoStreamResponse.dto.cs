using System.Text.Json.Serialization;

namespace movie_library.DTO;

public class VideoStreamResponseDto
{
    [JsonPropertyName("streamUrl")]
    public string? StreamUrl { get; set; }
}