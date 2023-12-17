using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace movie_library.Models;

public enum RentalStatus
{
    Available = 1,
    Expired = 2
}

public enum PlanId
{
    One = 1,
    Three = 2,
    Unlimited = 3
}

public class RentalMovie : BaseModel
{
    [BsonElement("user")]
    [JsonPropertyName("user")]
    [Required]
    public User User { get; init; } = null!;
    
    [BsonElement("movie")]
    [JsonPropertyName("movie")]
    [Required]
    public Movie Movie { get; init; } = null!;
    
    [BsonElement("planId")]
    [JsonPropertyName("planId")]
    [Required]
    public PlanId PlanId { get; set; }
    
    [BsonElement("amountOfPlays")]
    [JsonPropertyName("amountOfPlays")]
    [Required]
    public int AmountOfPlays { get; set; }

    [BsonElement("rentalStatus")]
    [JsonPropertyName("rentalStatus")]
    [Required]
    public RentalStatus RentalStatus { get; init; } = RentalStatus.Available;
}