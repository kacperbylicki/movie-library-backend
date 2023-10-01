namespace movie_library.Exceptions;

[Serializable]
public class NewPasswordRequiredException : Exception
{
    public NewPasswordRequiredException() : base($"New password is required") {}
}