using Microsoft.Extensions.Options;
using MongoDB.Driver;
using movie_library.Config;
using movie_library.Models;
using movie_library.Repositories.Utils;

namespace movie_library.Repositories;

public class FavoritesRepository
{
    private readonly IMongoCollection<Favorite> _favoritesCollection;

    public FavoritesRepository(
        IMongoDatabase mongoDatabase,
        IOptions<DatabaseConfig> databaseConfig
    )
    {
        _favoritesCollection = mongoDatabase.GetCollection<Favorite>(databaseConfig.Value.FavoritesCollectionName);
    }
    
    public async Task<List<Favorite>> GetUserFavoriteMovies(string userId) => await _favoritesCollection.Find(x => x.UserId == userId).ToListAsync();

    public async Task UpsertFavoriteMovie(Favorite favoriteMovie)
    {
        var filter = FavoritesRepositoryUtils.GetUpsertFilter(favoriteMovie);
        var update = FavoritesRepositoryUtils.GetUpsertUpdate(favoriteMovie);
        var options = new UpdateOptions { IsUpsert = true };

        await _favoritesCollection.UpdateOneAsync(filter, update, options);
    }
}