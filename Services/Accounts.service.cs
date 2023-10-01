using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.Extensions.Options;
using movie_library.Config;
using movie_library.DTO;
using movie_library.Exceptions;
using movie_library.Mappers;
using movie_library.Models;

namespace movie_library.Services;

public class AccountsService
{
    private readonly AWSConfig _config;
    private readonly IAmazonCognitoIdentityProvider _identityProvider;
    private readonly CognitoUserPool _userPool;

    private string GetSecretHash(string value)
    {
        using HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_config.AppClientSecret));
        byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value + _config.AppClientId));
        return Convert.ToBase64String(hash);
    }

    public AccountsService(IOptions<AWSConfig> config, IAmazonCognitoIdentityProvider identityProvider,
        CognitoUserPool userPool)
    {
        _config = config.Value;
        _identityProvider = identityProvider;
        _userPool = userPool;
    }

    public async Task<Tokens> LoginAsync(LoginDto dto)
    {
        var user = new CognitoUser(dto.Email, clientID: _config.AppClientId, _userPool, _identityProvider,
            clientSecret: _config.AppClientSecret);

        var authRequest = new InitiateSrpAuthRequest()
        {
            Password = dto.Password
        };
        var authResponse = await user.StartWithSrpAuthAsync(authRequest);

        if (authResponse.ChallengeName == ChallengeNameType.NEW_PASSWORD_REQUIRED)
        {
            throw new NewPasswordRequiredException();
        }

        var tokens = TokensMapper.FromAuthResponseToDomain(authResponse);

        return tokens;
    }

    public async Task<User> GetUserAsync(string accessToken)
    {
        // Remove the "Bearer " prefix from the access token
        accessToken = accessToken.Replace("Bearer ", "");

        GetUserRequest request = new GetUserRequest
        {
            AccessToken = accessToken,
        };

        var response = await _identityProvider.GetUserAsync(request);

        var user = UserMapper.FromGetUserResponseToDomain(response);

        return user;
    }

    public async Task<List<string>> GetUserGroupsAsync(string email)
    {
        var request = new AdminListGroupsForUserRequest
        {
            UserPoolId = _userPool.PoolID,
            Username = email
        };

        var response = await _identityProvider.AdminListGroupsForUserAsync(request);

        return response.Groups.ConvertAll(group => group.GroupName);
    }

    public async Task<SignUpResponse> RegisterAsync(RegisterDto dto)
    {
        var signUpRequest = new SignUpRequest
        {
            ClientId = _config.AppClientId,
            SecretHash = GetSecretHash(dto.Email),
            Username = dto.Email,
            Password = dto.Password,
            UserAttributes = new List<AttributeType>
            {
                new() { Name = "email", Value = dto.Email },
                new() { Name = "nickname", Value = dto.Username },
            }
        };

        var response = await _identityProvider.SignUpAsync(signUpRequest);

        return response;
    }

    public async Task ConfirmAccountAsync(ConfirmAccountDto dto)
    {
        var confirmSignUpRequest = new ConfirmSignUpRequest
        {
            ClientId = _config.AppClientId,
            SecretHash = GetSecretHash(dto.Email),
            ConfirmationCode = dto.ConfirmationCode,
            Username = dto.Email,
        };

        await _identityProvider.ConfirmSignUpAsync(confirmSignUpRequest);
    }

    public async Task<Tokens> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var refreshTokenRequest = new AdminInitiateAuthRequest
        {
            UserPoolId = _config.UserPoolId,
            ClientId = _config.AppClientId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "REFRESH_TOKEN", dto.RefreshToken },
                { "SECRET_HASH", GetSecretHash(dto.UserId) }
            }
        };
        var refreshTokenResponse = await _identityProvider.AdminInitiateAuthAsync(refreshTokenRequest);

        var tokens = TokensMapper.FromAdminInitiateAuthResponseToDomain(refreshTokenResponse, dto.RefreshToken);

        return tokens;
    }

    public async Task LogoutAsync(string accessToken)
    {
        // Remove the "Bearer " prefix from the access token
        accessToken = accessToken.Replace("Bearer ", "");

        var request = new GlobalSignOutRequest
        {
            AccessToken = accessToken
        };

        await _identityProvider.GlobalSignOutAsync(request);
    }

    public async Task<ResendConfirmationCodeResponseDto> ResendConfirmationCode(ResendConfirmationCodeDto dto)
    {
        var request = new ResendConfirmationCodeRequest
        {
            ClientId = _config.AppClientId,
            SecretHash = GetSecretHash(dto.Email),
            Username = dto.Email,
        };

        var response = await _identityProvider.ResendConfirmationCodeAsync(request);

        return new ResendConfirmationCodeResponseDto
        {
            Destination = response.CodeDeliveryDetails.Destination,
        };
    }
}
        