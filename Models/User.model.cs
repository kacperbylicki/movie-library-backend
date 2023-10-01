using System.Text.Json.Serialization;

namespace movie_library.Models;

public class User
{
    [JsonPropertyName("uuid")]
    public string Id { get; init; }
    
    [JsonPropertyName("email")]
    public string Email { get; init; }
    
    [JsonPropertyName("username")]
    public string Username { get; init; }
    
    [JsonPropertyName("accountVerified")]
    public string AccountVerified { get; init; }
}