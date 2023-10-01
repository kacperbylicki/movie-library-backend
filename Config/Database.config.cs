namespace movie_library.Config;

public class DatabaseConfig
{
    public string Database { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string MoviesCollectionName { get; set; }
    public string FavoritesCollectionName { get; set; }
    public string RentalsCollectionName { get; set; }
    public string ConnectionString { get; set; }

    public DatabaseConfig()
    {
        Database = Environment.GetEnvironmentVariable("DB_NAME");
        Host = Environment.GetEnvironmentVariable("DB_HOST");
        Port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT") ?? "27017");
        User = Environment.GetEnvironmentVariable("DB_USER");
        Password = Environment.GetEnvironmentVariable("DB_PASS");
        MoviesCollectionName = Environment.GetEnvironmentVariable("DB_MOVIES_COLLECTION_NAME");
        FavoritesCollectionName = Environment.GetEnvironmentVariable("DB_FAVORITES_COLLECTION_NAME");
        RentalsCollectionName = Environment.GetEnvironmentVariable("DB_RENTALS_COLLECTION_NAME");
        ConnectionString = Environment.GetEnvironmentVariable("DB_URI") ?? $@"mongodb://{User}:{Password}@{Host}:{Port}";
    }
}