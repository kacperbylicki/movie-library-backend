using Microsoft.Extensions.Options;
using MongoDB.Driver;
using movie_library.Config;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Mappers;
using movie_library.Models;
using movie_library.Repositories;

namespace movie_library.Services;

public class RentalsService
{
    private readonly RentalsRepository _rentalsRepository;
    private readonly MoviesRepository _moviesRepository;

    public RentalsService(IOptions<DatabaseConfig> databaseConfig)
    {
        var mongoClient = new MongoClient(databaseConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseConfig.Value.Database);
        
        _rentalsRepository = new RentalsRepository(mongoDatabase, databaseConfig);
        _moviesRepository = new MoviesRepository(mongoDatabase, databaseConfig);
    }

    public async Task RentMovieAsync(User user, string movieId, RentMovieDto dto)
    {
        var movie = await _moviesRepository.GetOneById(movieId);

        if (movie is null)
        {
            throw new MovieNotFoundException();
        }

        var isMovieAlreadyRentedByUser = await _rentalsRepository.IsMovieAlreadyRentedByUserAsync(user, movieId);

        if (isMovieAlreadyRentedByUser)
        {
            throw new MovieIsAlreadyRentedException();
        }

        try
        {
            var rentalMovie = MovieRentalMapper.FromDtoToDomain(dto, movie, user);
            await _rentalsRepository.RentMovieAsync(rentalMovie);   
        }
        catch
        {
            throw new MovieCouldNotBeRentedException();
        }
    }

    public async Task<IEnumerable<RentalMovie>> GetRentedMoviesAsync(User user)
    {
        return await _rentalsRepository.GetRentedMoviesByUserIdAsync(user.Id);
    }
    
    public async Task<string> GetRentedMovieVideoKeyAsync(User user, string movieId)
    {
        var isMovieAlreadyRentedByUser = await _rentalsRepository.IsMovieAlreadyRentedByUserAsync(user, movieId);
        if (!isMovieAlreadyRentedByUser)
        {
            throw new MovieIsNotRentedException();
        }

        var isMovieRentalExpired = await _rentalsRepository.IsMovieRentalExpiredAsync(user, movieId);
        if (isMovieRentalExpired)
        {
            throw new MovieRentalExpiredException();
        }
        
        var rentedMovie = await _rentalsRepository.GetRentedMovieByUserIdAndMovieIdAsync(user.Id, movieId);

        if (rentedMovie.PlanId == PlanId.Unlimited)
        {
            return rentedMovie.Movie.VideoStreamKey;
        }
        
        await _rentalsRepository.DecreaseMovieRentalAmountOfPlaysAsync(rentedMovie);
        return rentedMovie.Movie.VideoStreamKey;
    }
}
