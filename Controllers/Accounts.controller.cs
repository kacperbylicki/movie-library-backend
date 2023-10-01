using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Models;
using movie_library.Services;

namespace movie_library.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> _logger;
    private readonly AccountsService _accountsService;

    public AccountsController(ILogger<AccountsController> logger, AccountsService accountsService)
    {
        _logger = logger;
        _accountsService = accountsService;
    }

    [HttpGet("current-user")]
    [Authorize]
    public async Task<ActionResult<User>> GetCurrentUser([FromHeader(Name = "Authorization")] string accessToken)
    {
        try
        {
            var user = await _accountsService.GetUserAsync(accessToken);

            return user;
        } 
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (UserNotFoundException)
        {
            return BadRequest();
        }
        catch (UserNotConfirmedException)
        {
            return Unauthorized();
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticatedUserResponseDto>> Login(LoginDto dto)
    {
        try
        {
            var tokens = await _accountsService.LoginAsync(dto);
            var user = await _accountsService.GetUserAsync(tokens.AccessToken);
            var groups = await _accountsService.GetUserGroupsAsync(dto.Email);

            return new AuthenticatedUserResponseDto()
            {
                Tokens = tokens,
                User = user,
                Groups = groups,
            };
        }
        catch (UserNotConfirmedException)
        {
            return Unauthorized(new {
                explanation = "Account not confirmed"
            });
        }
        catch (NewPasswordRequiredException)
        {
            return Unauthorized(new
            {
                explanation = "Password reset required"
            });
        }
        catch (UserNotFoundException)
        {
            return Unauthorized(new
            {
                explanation = "Invalid username or password"
            });
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized(new
            {
                explanation = "Invalid username or password"
            });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        try
        {
            var registerAccountResponse = await _accountsService.RegisterAsync(dto);

            return Created($"/api/accounts/register", new RegisteredUserResponseDto()
            {
                UserId = registerAccountResponse.UserSub,
                UserConfirmed = registerAccountResponse.UserConfirmed,
                Destination = registerAccountResponse.CodeDeliveryDetails.Destination
            });
        }
        catch (UsernameExistsException)
        {
            return BadRequest(new
            {
                explanation = "Unknown error occurred"
            });
        }
        catch (InvalidPasswordException)
        {
            return BadRequest(new
            {
                explanation = "Password not meets minimal requirements"
            });
        }
        catch (InvalidParameterException)
        {
            return BadRequest(new
            {
                explanation = "One of the provided parameters is wrong"
            });
        }
    }
    
    [HttpPost("confirm")]
    public async Task<ActionResult> ConfirmAccount(ConfirmAccountDto dto)
    {
        try
        {
            await _accountsService.ConfirmAccountAsync(dto);

            return Ok();
        }
        catch (LimitExceededException)
        {
            return BadRequest();
        }
        catch (UserNotFoundException)
        {
            return BadRequest();
        }
        catch (ExpiredCodeException)
        {
            return BadRequest(new
            {
                explanation = "Confirmation code expired"
            });
        }
        catch (InvalidParameterException)
        {
            return BadRequest();
        }
        catch (NotAuthorizedException)
        {
            return BadRequest(new
            {
                explanation = "Account has been already confirmed"
            });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<Tokens>> RefreshToken(RefreshTokenDto dto)
    {
        try
        {
            var tokens = await _accountsService.RefreshTokenAsync(dto);

            return tokens;
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (UserNotFoundException)
        {
            return BadRequest();
        }
        catch (InvalidParameterException)
        {
            return BadRequest();
        }
    }
    
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromHeader(Name = "Authorization")] string accessToken)
    {
        try
        {
            await _accountsService.LogoutAsync(accessToken);

            return Ok();
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (InvalidParameterException)
        {
            return BadRequest();
        }
    }

    [HttpPost("confirm/resend")]
    public async Task<ActionResult<ResendConfirmationCodeResponseDto>> ResendConfirmationCode(ResendConfirmationCodeDto dto)
    {
        try
        {
            var response = await _accountsService.ResendConfirmationCode(dto);

            return response;
        }
        catch (CodeDeliveryFailureException)
        {
            return UnprocessableEntity();
        }
        catch (InternalErrorException)
        {
            return UnprocessableEntity();
        }
        catch (InvalidParameterException)
        {
            return BadRequest();
        }
        catch (LimitExceededException)
        {
            return new StatusCodeResult(429);
        }
        catch (NotAuthorizedException)
        {
            return Unauthorized();
        }
        catch (ResourceNotFoundException)
        {
            return UnprocessableEntity();
        }
        catch (TooManyRequestsException)
        {
            return new StatusCodeResult(429);
        }
        catch (UserNotFoundException)
        {
            return BadRequest();
        }
    }
}