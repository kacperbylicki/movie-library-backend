using Microsoft.Extensions.Options;
using MongoDB.Driver;
using movie_library.Config;
using movie_library.Interfaces;
using movie_library.Models;
using movie_library.Repositories.Utils;

namespace movie_library.Repositories
{
    public class MoviesRepository : IRepository<Movie>
    {
        private readonly IMongoCollection<Movie> _moviesCollection;

        public MoviesRepository(
            IMongoDatabase mongoDatabase,
            IOptions<DatabaseConfig> databaseConfig
        )
        {
            _moviesCollection = mongoDatabase.GetCollection<Movie>(databaseConfig.Value.MoviesCollectionName);
        }

        public async Task<List<Movie>> GetAll() => await _moviesCollection.Find(_ => true).ToListAsync();
        public async Task<Movie?> GetOneById(string id) => await _moviesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<Movie?> GetOneByTitle(string title) => await _moviesCollection.Find(x => x.Title == title).FirstOrDefaultAsync();

        public async Task CreateOne(Movie movie) => await _moviesCollection.InsertOneAsync(movie);

        public async Task UpdateOne(string id, Movie movie) => await _moviesCollection.ReplaceOneAsync(x => x.Id == id, movie);

        public async Task DeleteOne(string id) => await _moviesCollection.DeleteOneAsync(x => x.Id == id);
        
        public async Task<Comment?> GetCommentById(string movieId, string commentId)
        {
            var filter = MoviesRepositoryUtils.GetCommentFilter(movieId, commentId);
            
            var movie = await _moviesCollection.Find(filter).FirstOrDefaultAsync();
            var comment = movie?.Comments.FirstOrDefault(c => c?.Id == commentId);

            return comment;
        }
        
        public async Task CreateComment(string movieId, Comment comment)
        {
            var filter = MoviesRepositoryUtils.GetMovieFilter(movieId);
            var update = MoviesRepositoryUtils.GetCreateUpdate("Comments", comment);

            await _moviesCollection.FindOneAndUpdateAsync(filter, update);
        }
        
        public async Task UpdateComment(string movieId, Comment comment)
        {
            var filter = MoviesRepositoryUtils.GetCommentFilter(movieId, comment.Id);
            var update = MoviesRepositoryUtils.GetUpdateUpdate("Comments", comment);

            await _moviesCollection.FindOneAndUpdateAsync(filter, update);
        }
        
        public async Task DeleteComment(string movieId, Comment comment)
        {
            var filter = MoviesRepositoryUtils.GetCommentFilter(movieId, comment.Id);
            var update = MoviesRepositoryUtils.GetDeleteUpdate("Comments", comment);

            await _moviesCollection.FindOneAndUpdateAsync(filter, update);
        }
        
        public async Task<Rating?> GetRatingById(string movieId, string ratingId)
        {
            var filter = MoviesRepositoryUtils.GetRatingFilter(movieId, ratingId);
            
            var movie = await _moviesCollection.Find(filter).FirstOrDefaultAsync();
            var rating = movie?.Ratings.FirstOrDefault(c => c?.Id == ratingId);

            return rating;
        }

        public async Task CreateRating(string movieId, Rating rating)
        {
            var filter = MoviesRepositoryUtils.GetMovieFilter(movieId);
            var update = MoviesRepositoryUtils.GetCreateUpdate("Ratings", rating);

            await _moviesCollection.FindOneAndUpdateAsync(filter, update);
        }
        
        public async Task UpdateRating(string movieId, Rating rating)
        {
            var filter = MoviesRepositoryUtils.GetRatingFilter(movieId, rating.Id);
            var update = MoviesRepositoryUtils.GetUpdateUpdate("Ratings", rating);

            await _moviesCollection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task DeleteRating(string movieId, Rating rating)
        {
            var filter = MoviesRepositoryUtils.GetRatingFilter(movieId, rating.Id);
            var update = MoviesRepositoryUtils.GetDeleteUpdate("Ratings", rating);
            
            await _moviesCollection.FindOneAndUpdateAsync(filter, update);
        }
    }
}