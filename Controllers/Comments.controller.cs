using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Services;

namespace movie_library.Controllers;

[ApiController]
[Route("api/movies/{movieId:length(24)}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ILogger<CommentsController> _logger;
    private readonly CommentsService _commentsService;
    private readonly AccountsService _accountsService;

    public CommentsController(ILogger<CommentsController> logger, CommentsService commentsService, AccountsService accountsService)
    {
        _logger = logger;
        _commentsService = commentsService;
        _accountsService = accountsService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromHeader(Name = "Authorization")] string accessToken, string movieId, CreateCommentDto dto)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            var comment = await _commentsService.CreateAsync(user, dto, movieId);
            
            return Created($"/api/movies/{movieId}/comments", comment);
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

    [HttpPut("{commentId:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Update([FromHeader(Name = "Authorization")] string accessToken, string movieId, string commentId, UpdateCommentDto dto)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            await _commentsService.UpdateAsync(user, dto, movieId, commentId);

            return NoContent();
        }
        catch (MovieNotFoundException)
        {
            return BadRequest();
        }
        catch (CommentNotFoundException)
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
    
    [HttpDelete("{commentId:length(24)}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromHeader(Name = "Authorization")] string accessToken, string movieId, string commentId)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            await _commentsService.DeleteAsync(user, movieId, commentId);
        
            return NoContent();
        }
        catch (CommentNotFoundException)
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