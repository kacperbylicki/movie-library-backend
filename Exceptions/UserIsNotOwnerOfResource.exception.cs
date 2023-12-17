namespace movie_library.Exceptions;

[Serializable]
public class UserIsNotOwnerOfResourceException : Exception
{
    public UserIsNotOwnerOfResourceException() : base("User is not owner of requested resource") {} 
}