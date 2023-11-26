using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using movie_library.Config;
using movie_library.DTO;
using movie_library.Models;

namespace movie_library.Repositories;

public class RentalsRepository
{
    private readonly IMongoCollection<RentalMovie> _rentalsCollection;

    public RentalsRepository(
        IMongoDatabase mongoDatabase,
        IOptions<DatabaseConfig> databaseConfig
    )
    {
        _rentalsCollection = mongoDatabase.GetCollection<RentalMovie>(databaseConfig.Value.RentalsCollectionName);
    }

    public async Task<IEnumerable<RentalMovie>> GetRentedMoviesByUserIdAsync(string userId)
    {
        var filter = Builders<RentalMovie>.Filter.Eq(rm => rm.User.Id, userId) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.RentalStatus, RentalStatus.Available);
        return await _rentalsCollection.Find(filter).ToListAsync();
    }
    
    public async Task<bool> IsMovieAlreadyRentedByUserAsync(User user, string movieId)
    {
        var filter = Builders<RentalMovie>.Filter.Eq(rm => rm.User.Id, user.Id) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.Movie.Id, movieId) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.RentalStatus, RentalStatus.Available);
        var rentalMovie = await _rentalsCollection.Find(filter).FirstOrDefaultAsync();
        return rentalMovie is not null;
    }
    
    public async Task<bool> IsMovieRentalExpiredAsync(User user, string movieId)
    {
        var filter = Builders<RentalMovie>.Filter.Eq(rm => rm.User.Id, user.Id) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.Movie.Id, movieId) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.RentalStatus, RentalStatus.Expired);
        var rentalMovie = await _rentalsCollection.Find(filter).FirstOrDefaultAsync();
        return rentalMovie is not null;
    }
    
    public async Task RentMovieAsync(RentalMovie rentalMovie)
    {
        var filter = Builders<RentalMovie>.Filter.Eq(r => r.Movie.Id, rentalMovie.Movie.Id);
        var options = new UpdateOptions { IsUpsert = true };
        var update = Builders<RentalMovie>.Update
            .Set(r => r.Movie, rentalMovie.Movie)
            .Set(r => r.User, rentalMovie.User)
            .Set(r => r.AmountOfPlays, rentalMovie.AmountOfPlays)
            .Set(r => r.RentalStatus, rentalMovie.RentalStatus);

        await _rentalsCollection.UpdateOneAsync(filter, update, options);
    }
    
    public async Task<RentalMovie> GetRentedMovieByUserIdAndMovieIdAsync(string userId, string movieId)
    {
        var filter = Builders<RentalMovie>.Filter.Eq(rm => rm.User.Id, userId) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.Movie.Id, movieId) &
                     Builders<RentalMovie>.Filter.Eq(rm => rm.RentalStatus, RentalStatus.Available);
        return await _rentalsCollection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task DecreaseMovieRentalAmountOfPlaysAsync(RentalMovie rentalMovie)
    {
        var filter = Builders<RentalMovie>.Filter.Eq(rm => rm.Id, rentalMovie.Id);
        var update = Builders<RentalMovie>.Update.Set(rm => rm.AmountOfPlays, rentalMovie.AmountOfPlays - 1);
        var options = new FindOneAndUpdateOptions<RentalMovie>
        {
            ReturnDocument = ReturnDocument.After
        };
    
        var updatedMovieRental = await _rentalsCollection.FindOneAndUpdateAsync(filter, update, options);

        if (updatedMovieRental is { AmountOfPlays: <= 0, RentalStatus: RentalStatus.Available })
        {
            await ExpireMovieRentalAsync(filter);
        }
    }

    private async Task ExpireMovieRentalAsync(FilterDefinition<RentalMovie>? filter)
    {
        var expiredRentalUpdate = Builders<RentalMovie>.Update.Set(rm => rm.RentalStatus, RentalStatus.Expired);
        await _rentalsCollection.UpdateOneAsync(filter, expiredRentalUpdate);
    }
}