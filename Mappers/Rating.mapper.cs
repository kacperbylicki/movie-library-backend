using MongoDB.Bson;
using movie_library.DTO;
using movie_library.Models;

namespace movie_library.Mappers;

public class RatingMapper
{
    public static Rating FromCreateDtoToDomain(CreateRatingDto dto, User user)
    {
        return new Rating()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            User = user,
            Rate = dto.Rate,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public static Rating FromUpdateDtoToDomain(UpdateRatingDto dto, Rating entity)
    {
        return new Rating()
        {
            Id = entity.Id,
            User =  entity.User,
            Rate = dto.Rate,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = DateTime.Now
        };
    }
}