using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace movie_library.Models;

public class Favorite
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("uuid")]
    public string Id { get; init; }
    
    [BsonElement("movieId")]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("movieId")]
    [Required]
    public string MovieId { get; init; }
    
    [BsonElement("userId")]
    [JsonPropertyName("userId")]
    [Required]
    public string UserId { get; init; }
    
    [BsonElement("isFavorite")]
    [JsonPropertyName("isFavorite")]
    [Required]
    public bool IsFavorite { get; init; }
}