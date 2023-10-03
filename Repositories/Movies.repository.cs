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
    }
}