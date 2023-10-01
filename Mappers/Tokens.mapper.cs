using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using movie_library.Models;

namespace movie_library.Mappers;

public class TokensMapper
{
    public static Tokens FromAuthResponseToDomain(AuthFlowResponse authResponse)
    {
        var expirationTimespan = TimeSpan.FromSeconds(authResponse.AuthenticationResult.ExpiresIn);
        var expiration = DateTimeOffset.Now + expirationTimespan;

        return new Tokens()
        {
            AccessToken = authResponse.AuthenticationResult.AccessToken,
            RefreshToken = authResponse.AuthenticationResult.RefreshToken,
            Expiration = expiration
        };
    }

    public static Tokens FromAdminInitiateAuthResponseToDomain(AdminInitiateAuthResponse authResponse, string? previousRefreshToken)
    {
        var expirationTimespan = TimeSpan.FromSeconds(authResponse.AuthenticationResult.ExpiresIn);
        var expiration = DateTimeOffset.Now + expirationTimespan;

        return new Tokens()
        {
            AccessToken = authResponse.AuthenticationResult.AccessToken,
            RefreshToken = authResponse.AuthenticationResult.RefreshToken ?? previousRefreshToken,
            Expiration = expiration
        };
    }
}