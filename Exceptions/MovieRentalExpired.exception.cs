namespace movie_library.Exceptions;

[Serializable]
public class MovieRentalExpiredException : Exception
{
    public MovieRentalExpiredException() : base("Movie rental expired") {} 
}