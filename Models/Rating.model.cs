using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace movie_library.Models
{
    public class Rating : BaseModel
    {
        [BsonElement("user")]
        [JsonPropertyName("user")]
        [Required]
        public User User { get; set; }
        
        [BsonElement("rate")]
        [JsonPropertyName("rate")]
        [Required, Range(0, 5)]
        public decimal Rate { get; set; }
    }
}