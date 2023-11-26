using Microsoft.Extensions.Options;
using MongoDB.Driver;
using movie_library.Config;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Helpers;
using movie_library.Mappers;
using movie_library.Models;
using movie_library.Repositories;

namespace movie_library.Services;

public class CommentsService
{
    private readonly MoviesRepository _moviesRepository;

    public CommentsService(
        IOptions<DatabaseConfig> databaseConfig)
    {
        var mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfig.Value.Database);
        
        _moviesRepository = new MoviesRepository(mongoDatabase, databaseConfig);
    }

    private async Task<Comment?> GetOneByIdAsync(string movieId, string commentId)
    {
        var comment = await _moviesRepository.GetCommentById(movieId, commentId);

        return comment;
    }

    public async Task<Comment> CreateAsync(User user, CreateCommentDto dto, string movieId)
    {
        var existingMovie = await _moviesRepository.GetOneById(movieId);

        if (existingMovie is null)
        {
            throw new MovieNotFoundException();
        }
        
        var comment = CommentMapper.FromCreateDtoToDomain(dto, user);
        
        await _moviesRepository.CreateComment(movieId, comment);

        return comment;
    }
    
    public async Task UpdateAsync(User user, UpdateCommentDto dto, string movieId, string commentId)
    {
        var existingComment = await GetOneByIdAsync(movieId, commentId);
        if (existingComment is null)
        {
            throw new CommentNotFoundException();
        }

        var isUserOwnerOfComment = AuthorizationHelpers.IsUserOwnerOfComment(user, existingComment);
        if (!isUserOwnerOfComment)
        {
            throw new UserIsNotOwnerOfResourceException();
        }
        
        var comment = CommentMapper.FromUpdateDtoToDomain(dto, existingComment);
        
        await _moviesRepository.UpdateComment(movieId, comment);
    }

    public async Task DeleteAsync(User user, string movieId, string commentId)
    {
        var comment = await GetOneByIdAsync(movieId, commentId);

        if (comment is null)
        {
            throw new CommentNotFoundException();
        }

        var isUserOwnerOfComment = AuthorizationHelpers.IsUserOwnerOfComment(user, comment);
        if (!isUserOwnerOfComment)
        {
            throw new UserIsNotOwnerOfResourceException();
        }

        await _moviesRepository.DeleteComment(movieId, comment);
    }
}