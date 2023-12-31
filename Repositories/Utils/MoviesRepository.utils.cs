using MongoDB.Bson;
using MongoDB.Driver;
using movie_library.Models;

namespace movie_library.Repositories.Utils;

public class MoviesRepositoryUtils
{
    public static FilterDefinition<Movie> GetMovieFilter(string movieId)
    {
        return Builders<Movie>.Filter.Eq("Id", movieId);
    }
    
    public static FilterDefinition<Movie> GetCommentFilter(string movieId, string commentId)
    {
        return
            Builders<Movie>.Filter.Eq("Id", movieId) & 
            Builders<Movie>.Filter.ElemMatch<Comment>(movie => movie.Comments!, x => x.Id == commentId);
    }

    public static FilterDefinition<Movie> GetRatingFilter(string movieId, string ratingId)
    {
        return
            Builders<Movie>.Filter.Eq("Id", movieId) &
            Builders<Movie>.Filter.ElemMatch<Rating>(movie => movie.Ratings!, x => x.Id == ratingId);
    }
    
    public static UpdateDefinition<Movie> GetCreateUpdate(string fieldName, object element)
    {
        return Builders<Movie>.Update.Push(fieldName, element);
    }
    
    public static UpdateDefinition<Movie> GetUpdateUpdate(string fieldName, object element)
    {
        return Builders<Movie>.Update.Set($"{fieldName}.$", element);
    }
    
    public static UpdateDefinition<Movie> GetDeleteUpdate(string fieldName, object element)
    {
        return Builders<Movie>.Update.Pull(fieldName, element);
    }
    
    public static UpdateDefinition<Movie> GetDecreaseStockUpdate()
    {
        return Builders<Movie>.Update.Inc("Stock", -1);
    }
    
    public static UpdateDefinition<Movie> GetIncreaseStockUpdate()
    {
        return Builders<Movie>.Update.Inc("Stock", 1);
    }
}