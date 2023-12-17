namespace movie_library.Exceptions;

[Serializable]
public abstract class NotFoundException : Exception
{
    protected NotFoundException() {}
    protected NotFoundException(string entity): base($"{entity} not found") {}
}

[Serializable]
public class MovieNotFoundException : NotFoundException
{
    public MovieNotFoundException(): base("movie") {}
}

[Serializable]
public class CommentNotFoundException : NotFoundException
{
    public CommentNotFoundException(): base("comment") {}
}

[Serializable]
public class RatingNotFoundException : NotFoundException
{
    public RatingNotFoundException(): base("rating") {}
}

