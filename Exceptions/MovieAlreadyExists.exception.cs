namespace movie_library.Exceptions;

[Serializable]
public class MovieAlreadyExistsException : Exception
{
    public MovieAlreadyExistsException() {}
    
    public MovieAlreadyExistsException(string title) : base($"Movie '{title}' already exists") {}
}