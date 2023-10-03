namespace movie_library.Interfaces
{
    public interface IService<T>
    {
        Task<List<T>> GetAsync();
        Task<T?> GetAsyncById(string id);
        Task<T> CreateAsync(T newObject);
        Task UpdateAsync(string id,T newObject);
        Task DeleteAsync(string id);
    }
}