using MongoDB.Bson;
using movie_library.DTO;
using movie_library.Models;

namespace movie_library.Mappers;

public class MovieRentalMapper
{
    public static RentalMovie FromDtoToDomain(RentMovieDto dto, Movie movie, User user)
    {
        var amountOfPlays = dto.PlanId switch
        {
            PlanId.One => 1,
            PlanId.Three => 3,
            _ => int.MaxValue
        };
        
        return new RentalMovie()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Movie = movie,
            User = user,
            PlanId = dto.PlanId,
            AmountOfPlays = amountOfPlays,
            RentalStatus = RentalStatus.Available,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
    }
}