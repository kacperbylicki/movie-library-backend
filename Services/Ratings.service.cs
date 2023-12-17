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

public class RatingsService
{
    private readonly MoviesRepository _moviesRepository;

    public RatingsService(
        IOptions<DatabaseConfig> databaseConfig)
    {
        var mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfig.Value.Database);
        
        _moviesRepository = new MoviesRepository(mongoDatabase, databaseConfig);
    }
    
    private async Task<Rating?> GetOneByIdAsync(string movieId, string ratingId)
    {
        var rating = await _moviesRepository.GetRatingById(movieId, ratingId);

        return rating;
    }

    public async Task<Rating> CreateAsync(User user, CreateRatingDto dto, string movieId)
    {
        var existingMovie = await _moviesRepository.GetOneById(movieId);

        if (existingMovie is null)
        {
            throw new MovieNotFoundException();
        }

        var rating = RatingMapper.FromCreateDtoToDomain(dto, user);
        
        await _moviesRepository.CreateRating(movieId, rating);

        return rating;
    }
    
    public async Task<Rating> UpdateAsync(User user, UpdateRatingDto dto, string movieId, string ratingId)
    {
        var existingRating = await GetOneByIdAsync(movieId, ratingId);
        if (existingRating is null)
        {
            throw new RatingNotFoundException();
        }
        
        var isUserOwnerOfRating = AuthorizationHelpers.IsUserOwnerOfRating(user, existingRating);
        if (!isUserOwnerOfRating)
        {
            throw new UserIsNotOwnerOfResourceException();
        }
        
        var rating = RatingMapper.FromUpdateDtoToDomain(dto, existingRating);
        
        await _moviesRepository.UpdateRating(movieId, rating);

        return rating;
    }

    public async Task DeleteAsync(User user, string movieId, string ratingId)
    {
        var rating = await GetOneByIdAsync(movieId, ratingId);
        if (rating is null)
        {
            throw new RatingNotFoundException();
        }
        
        var isUserOwnerOfRating = AuthorizationHelpers.IsUserOwnerOfRating(user, rating);
        if (!isUserOwnerOfRating)
        {
            throw new UserIsNotOwnerOfResourceException();
        }

        await _moviesRepository.DeleteRating(movieId, rating);
    }
}