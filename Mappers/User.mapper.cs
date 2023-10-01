using Amazon.CognitoIdentityProvider.Model;
using movie_library.Models;

namespace movie_library.Mappers;

public class UserMapper
{
    public static User FromGetUserResponseToDomain(GetUserResponse data)
    {
        return new User()
        {
            Id = data.Username,
            Email = data.UserAttributes.FirstOrDefault(a => a.Name == "email")?.Value ?? string.Empty,
            Username = data.UserAttributes.FirstOrDefault(a => a.Name == "nickname")?.Value ?? string.Empty,
            AccountVerified = data.UserAttributes.FirstOrDefault(a => a.Name == "email_verified")?.Value ?? string.Empty,
        };
    }
}