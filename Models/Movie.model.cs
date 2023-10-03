using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using movie_library.Services;

namespace movie_library.Models;
public class Movie : BaseModel
{
    [BsonElement("title")]
    [JsonPropertyName("title")]
    [Required]
    public string Title { get; init; } = null!;

    [BsonElement("posterUrl")]
    [JsonPropertyName("posterUrl")]
    [Required]
    public string PosterUrl { get; init; } = null!;
    
    [BsonElement("videoStreamKey")]
    [JsonPropertyName("videoStreamKey")]
    [Required]
    public string VideoStreamKey { get; init; } = null!;

    [BsonElement("genre")]
    [JsonPropertyName("genre")]
    [Required]
    public List<string> Genre { get; init; } = null!;

    [BsonElement("producers")]
    [JsonPropertyName("producers")]
    [Required]
    public List<string> Producers { get; init; } = null!;

    [BsonElement("directors")]
    [JsonPropertyName("directors")]
    [Required]
    public List<string> Directors { get; init; } = null!;

    [BsonElement("roles")]
    [JsonPropertyName("roles")]
    [Required]
    public List<Role> Roles { get; init; } = null!;

    [BsonElement("releaseYear")]
    [JsonPropertyName("releaseYear")]
    [Range(1900, 2023)]
    [Required]
    public int ReleaseYear { get; init; }
}