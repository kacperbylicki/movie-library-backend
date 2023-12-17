using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Models;
using movie_library.Services;

namespace movie_library.Controllers;

[ApiController]
[Route("api/rentals/")]
public class RentalsController : ControllerBase
{
    private readonly ILogger<RentalsController> _logger;
    private readonly RentalsService _rentalsService;
    private readonly AccountsService _accountsService;

    public RentalsController(ILogger<RentalsController> logger, RentalsService rentalsService, AccountsService accountsService, VideosService videosService)
    {
        _logger = logger;
        _rentalsService = rentalsService;
        _accountsService = accountsService;
    }
    
    [HttpPost("{movieId:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Rent([FromHeader(Name = "Authorization")] string accessToken, string movieId, RentMovieDto dto)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            await _rentalsService.RentMovieAsync(user, movieId, dto);

            return NoContent();
        }
        catch (MovieIsAlreadyRentedException)
        {
            return UnprocessableEntity(new
            {
                explanation = "Movie is already rented by the user"
            });
        }
        catch (MovieNotFoundException)
        {
            return BadRequest();
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (UserNotFoundException)
        {
            return Forbid();
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<RentalMovie>> GetRentedMovies([FromHeader(Name = "Authorization")] string accessToken)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            var rentedMovies = await _rentalsService.GetRentedMoviesAsync(user);

            return Ok(rentedMovies);
        } 
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (UserNotFoundException)
        {
            return Forbid();
        }
    }

    [HttpGet("{movieId:length(24)}/stream")]
    [Authorize]
    public async Task<ActionResult<VideoStreamResponseDto>> StreamVideo([FromHeader(Name = "Authorization")] string accessToken, string movieId)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);
            var videoKey = await _rentalsService.GetRentedMovieVideoKeyAsync(user, movieId);

            return new VideoStreamResponseDto
            {
                StreamUrl = $"https://ik.imagekit.io/kvi3ocigc/{videoKey}"
            };
        }
        catch (MovieIsNotRentedException)
        {
            return UnprocessableEntity();
        }
        catch (MovieRentalExpiredException)
        {
            return UnprocessableEntity();
        }
        catch (MovieNotFoundException)
        {
            return BadRequest();
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (Exception)
        {
            return UnprocessableEntity();
        }
    }
}