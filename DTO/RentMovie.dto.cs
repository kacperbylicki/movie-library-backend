using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using movie_library.Models;

namespace movie_library.DTO;

public class RentMovieDto
{
    [JsonPropertyName("planId")]
    [Required]
    public PlanId PlanId { get; set; } = default!;
}