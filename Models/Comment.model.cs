using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace movie_library.Models;

public class Comment : BaseModel
{
    [BsonElement("user")]
    [JsonPropertyName("user")]
    [Required]
    public User User { get; set; }

    [BsonElement("content")]
    [JsonPropertyName("content")]
    [Required]
    public string Content { get; init; }
}