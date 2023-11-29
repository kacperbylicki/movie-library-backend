using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Models;
using movie_library.Services;

namespace movie_library.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly MoviesService _moviesService;
        private readonly AccountsService _accountsService;
        private readonly FavoritesService _favoritesService;
        
        public MoviesController(ILogger<MoviesController> logger, MoviesService moviesService, AccountsService accountsService, FavoritesService favoritesService)
        {
            _logger = logger;
            _moviesService = moviesService;
            _accountsService = accountsService;
            _favoritesService = favoritesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetByTitle(string? title)
        {
            try
            {
                if (title is null) return await _moviesService.GetAllAsync();
            
                var movie = await _moviesService.GetOneByTitleAsync(title);

                return new List<Movie> {movie};
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{movieId:length(24)}")]
        public async Task<ActionResult<Movie>> GetById(string movieId)
        {
            try
            {
                var movie = await _moviesService.GetOneByIdAsync(movieId);

                return movie;
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrModeratorOnly")]
        public async Task<IActionResult> Create(CreateMovieDto dto)
        {
            try
            {
                var movie = await _moviesService.CreateAsync(dto);

                return Created($"/api/movies", movie);
            }
            catch (MovieAlreadyExistsException)
            {
                return Conflict();
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }

        [HttpPut("{movieId:length(24)}")]
        [Authorize(Policy = "AdminOrModeratorOnly")]
        public async Task<IActionResult> Update(string movieId, UpdateMovieDto dto)
        {
            try
            {
                await _moviesService.UpdateAsync(movieId, dto);

                return NoContent();
            } 
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{movieId:length(24)}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string movieId)
        {
            try
            {
                await _moviesService.DeleteAsync(movieId);

                return NoContent();
            }
            catch (MovieNotFoundException)
            {
                return NotFound();
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }
        
        [HttpPost("{movieId:length(24)}/favorite")]
        [Authorize]
        public async Task<IActionResult> UpsertFavoriteMovie([FromHeader(Name = "Authorization")] string accessToken, string movieId, FavoriteDto dto)
        {
            try
            {
                var user = await _accountsService.GetUserAsync(accessToken);
                
                await _favoritesService.UpsertFavoriteMovieAsync(user, dto, movieId);

                return Ok();
            }
            catch (MovieNotFoundException)
            {
                return UnprocessableEntity();
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }
        
        [HttpGet("favorites")]
        [Authorize]
        public async Task<ActionResult<List<Favorite>>> GetUserFavoriteMovies([FromHeader(Name = "Authorization")] string accessToken)
        {
            try
            {
                var user = await _accountsService.GetUserAsync(accessToken);
                
                var favorites = await _favoritesService.GetUserFavoriteMoviesAsync(user.Id);

                return favorites;
            }
            catch (UserNotFoundException)
            {
                return Forbid();
            }
        }
    }
}