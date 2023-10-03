namespace movie_library.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T?> GetOneById(string id);
        Task CreateOne(T newObject);
        Task UpdateOne(string id, T newObject);
        Task DeleteOne(string id);
    }
}