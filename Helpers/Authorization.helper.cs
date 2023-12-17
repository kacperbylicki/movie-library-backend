using MongoDB.Bson;
using movie_library.Models;

namespace movie_library.Helpers;

public class AuthorizationHelpers
{
    public static Boolean IsUserOwnerOfComment(User user, Comment comment)
    {

        return user.Id == comment.User.Id;
    }
    
    public static Boolean IsUserOwnerOfRating(User user, Rating rating)
    {

        return user.Id == rating.User.Id;
    }
}