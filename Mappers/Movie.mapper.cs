using MongoDB.Bson;
using movie_library.DTO;
using movie_library.Models;
using movie_library.Services;

namespace movie_library.Mappers;

public class MovieMapper
{
    public static Movie FromCreateDtoToDomain(CreateMovieDto dto, string posterUrl)
    {
        return new Movie()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Title = dto.Title,
            PosterUrl = posterUrl,
            VideoStreamKey = dto.VideoStreamKey,
            Genre = dto.Genre,
            Producers = dto.Producers,
            Directors = dto.Directors,
            Roles = dto.Roles,
            ReleaseYear = dto.ReleaseYear,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public static Movie FromUpdateDtoToDomain(UpdateMovieDto dto, Movie entity)
    {
        return new Movie()
        {
            Id = entity.Id,
            Title = dto.Title ?? entity.Title,
            PosterUrl = dto.Poster ?? entity.PosterUrl,
            VideoStreamKey = dto.VideoStreamKey ?? entity.VideoStreamKey,
            Genre = dto.Genre ?? entity.Genre,
            Producers = dto.Producers ?? entity.Producers,
            Directors = dto.Directors ?? entity.Directors,
            Roles = dto.Roles ?? entity.Roles,
            ReleaseYear = dto.ReleaseYear ?? entity.ReleaseYear,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = DateTime.Now
        };
    }
}