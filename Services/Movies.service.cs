using Microsoft.Extensions.Options;
using MongoDB.Driver;
using movie_library.Models;
using movie_library.Config;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Mappers;
using movie_library.Repositories;

namespace movie_library.Services;
public class MoviesService
{
    private readonly MoviesRepository _moviesRepository;
    private readonly ImagesService _imagesService;

    public MoviesService(
        IOptions<DatabaseConfig> databaseConfig, ImagesService imagesService)
    {
        var mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfig.Value.Database);
        
        _moviesRepository = new MoviesRepository(mongoDatabase, databaseConfig);
        _imagesService = imagesService;
    }

    private async Task<bool> ExistsAsync(string title)
    {
        var movie = await _moviesRepository.GetOneByTitle(title);
        return movie is not null;
    }
    
    public async Task<List<Movie>> GetAllAsync() => await _moviesRepository.GetAll();

    public async Task<Movie> GetOneByIdAsync(string movieId)
    {
        var movie = await _moviesRepository.GetOneById(movieId);

        if (movie is null)
        {
            throw new MovieNotFoundException();
        }

        return movie;
    }

    public async Task<Movie> GetOneByTitleAsync(string title)
    {
        var movie = await _moviesRepository.GetOneByTitle(title);

        if (movie is null)
        {
            throw new MovieNotFoundException();
        }

        return movie;
    }

    public async Task<Movie> CreateAsync(CreateMovieDto dto)
    {
        var movieExists = await ExistsAsync(dto.Title);

        if (movieExists)
        {
            throw new MovieAlreadyExistsException();
        }
        
        var posterUrl = await _imagesService.UploadAsync(dto.Poster);
        var movie = MovieMapper.FromCreateDtoToDomain(dto, posterUrl);

        await _moviesRepository.CreateOne(movie);

        return movie;
    }

    public async Task UpdateAsync(string movieId, UpdateMovieDto dto)
    {
        var existingMovie = await GetOneByIdAsync(movieId);

        if (existingMovie is null)
        {
            throw new MovieNotFoundException();
        }
        
        var movie = MovieMapper.FromUpdateDtoToDomain(dto, existingMovie);
        
        await _moviesRepository.UpdateOne(movieId, movie);
    }

    public async Task DeleteAsync(string movieId)
    {
        var movie = await GetOneByIdAsync(movieId);

        if (movie is null)
        {
            throw new MovieNotFoundException();
        }
        
        await _moviesRepository.DeleteOne(movieId);
    }
}