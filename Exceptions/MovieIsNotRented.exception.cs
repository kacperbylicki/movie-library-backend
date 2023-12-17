namespace movie_library.Exceptions;

[Serializable]
public class MovieIsNotRentedException : Exception
{
    public MovieIsNotRentedException() : base("Requested movie is not rented by the user") {} 
}