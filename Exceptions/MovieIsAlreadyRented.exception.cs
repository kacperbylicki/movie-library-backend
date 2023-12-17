namespace movie_library.Exceptions;

[Serializable]
public class MovieIsAlreadyRentedException : Exception
{
    public MovieIsAlreadyRentedException() : base("Requested movie is already rented by the user") {} 
}