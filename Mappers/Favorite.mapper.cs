using MongoDB.Bson;
using movie_library.DTO;
using movie_library.Models;

namespace movie_library.Mappers;

public class FavoriteMapper
{
    public static Favorite FromDtoToDomain(FavoriteDto dto, User user, string movieId)
    {
        return new Favorite()
        {
            UserId = user.Id,
            MovieId = movieId,
            IsFavorite = dto.IsFavorite,
        };
    }
}