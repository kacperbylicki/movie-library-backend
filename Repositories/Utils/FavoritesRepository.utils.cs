using MongoDB.Driver;
using movie_library.Models;

namespace movie_library.Repositories.Utils;

public class FavoritesRepositoryUtils
{
    public static FilterDefinition<Favorite> GetUpsertFilter(Favorite favoriteMovie)
    {
        return Builders<Favorite>.Filter.Eq("UserId", favoriteMovie.UserId) & Builders<Favorite>.Filter.Eq("MovieId", favoriteMovie.MovieId);
    }
    
    public static UpdateDefinition<Favorite> GetUpsertUpdate(Favorite favoriteMovie)
    {
        return Builders<Favorite>.Update.Set("UserId", favoriteMovie.UserId).Set("MovieId", favoriteMovie.MovieId).Set("IsFavorite", favoriteMovie.IsFavorite);
    }
}