using Microsoft.Extensions.Options;
using MongoDB.Driver;
using movie_library.Config;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Mappers;
using movie_library.Models;
using movie_library.Repositories;

namespace movie_library.Services;

public class FavoritesService
{
    private readonly FavoritesRepository _favoritesRepository;
    private readonly MoviesRepository _moviesRepository;

    public FavoritesService(
        IOptions<DatabaseConfig> databaseConfig)
    {
        var mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfig.Value.Database);
        
        _moviesRepository = new MoviesRepository(mongoDatabase, databaseConfig);
        _favoritesRepository = new FavoritesRepository(mongoDatabase, databaseConfig);
    }
    
    public async Task<List<Favorite>> GetUserFavoriteMoviesAsync(string userId) => await _favoritesRepository.GetUserFavoriteMovies(userId);
    public async Task UpsertFavoriteMovieAsync(User user, FavoriteDto dto, string movieId)
    {
        var movie = await _moviesRepository.GetOneById(movieId);

        if (movie is null)
        {
            throw new MovieNotFoundException();
        }

        var favoriteMovie = FavoriteMapper.FromDtoToDomain(dto, user, movieId);

        await _favoritesRepository.UpsertFavoriteMovie(favoriteMovie);
    }
}