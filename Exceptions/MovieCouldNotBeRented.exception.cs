namespace movie_library.Exceptions;

[Serializable]
public class MovieCouldNotBeRentedException : Exception
{
    public MovieCouldNotBeRentedException() : base("Movie could not be rented") {}
}