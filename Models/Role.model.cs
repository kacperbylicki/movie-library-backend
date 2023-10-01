using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace movie_library.Models
{
    public class Role
    {
        [BsonElement("actor")]
        [JsonPropertyName("actor")]
        [Required]
        public string? Actor { get; set; }

        [BsonElement("character")]
        [JsonPropertyName("character")]
        [Required]
        public string? Character { get; set; }
    }
}