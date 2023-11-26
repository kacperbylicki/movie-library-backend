using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Services;

namespace movie_library.Controllers;

[ApiController]
[Route("api/movies/{movieId:length(24)}/ratings")]
public class RatingsController : ControllerBase
{
    private readonly ILogger<RatingsController> _logger;
    private readonly RatingsService _ratingsService;
    private readonly AccountsService _accountsService;

    public RatingsController(ILogger<RatingsController> logger, RatingsService ratingsService, AccountsService accountsService)
    {
        _logger = logger;
        _ratingsService = ratingsService;
        _accountsService = accountsService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromHeader(Name = "Authorization")] string accessToken, string movieId, CreateRatingDto dto)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            var rating = await _ratingsService.CreateAsync(user, dto, movieId);
            
            return Created($"/api/movies/{movieId}/ratings", rating);
        }
        catch (MovieNotFoundException)
        {
            return BadRequest();
        }
        catch (UserNotFoundException)
        {
            return Forbid();
        }
    }
    
    [HttpPut("{ratingId:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update([FromHeader(Name = "Authorization")] string accessToken, string movieId, string ratingId, UpdateRatingDto dto)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);
            
            await _ratingsService.UpdateAsync(user, dto, movieId, ratingId);
        
            return NoContent();
        }
        catch (MovieNotFoundException)
        {
            return BadRequest();
        }
        catch (RatingNotFoundException)
        {
            return NotFound();
        }
        catch (UserIsNotOwnerOfResourceException)
        {
            return Forbid();
        }
        catch (UserNotFoundException)
        {
            return Forbid();
        }
    }
    
    [HttpDelete("{ratingId:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromHeader(Name = "Authorization")] string accessToken, string movieId, string ratingId)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);
            
            await _ratingsService.DeleteAsync(user, movieId, ratingId);
            
            return NoContent();
        }
        catch (MovieNotFoundException)
        {
            return BadRequest();
        }
        catch (RatingNotFoundException)
        {
            return NotFound();
        }
        catch (UserIsNotOwnerOfResourceException)
        {
            return Forbid();
        }
        catch (UserNotFoundException)
        {
            return Forbid();
        }
    }
}