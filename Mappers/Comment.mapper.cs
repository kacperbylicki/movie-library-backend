using MongoDB.Bson;
using movie_library.DTO;
using movie_library.Models;

namespace movie_library.Mappers;

public class CommentMapper
{
    public static Comment FromCreateDtoToDomain(CreateCommentDto dto, User user)
    {
        return new Comment()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            User = user,
            Content = dto.Content,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }

    public static Comment FromUpdateDtoToDomain(UpdateCommentDto dto, Comment entity)
    {
        return new Comment()
        {
            Id = entity.Id,
            User = entity.User,
            Content = dto.Content,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = DateTime.Now
        };
    }
}