using System.Text.Json.Serialization;
using JWT.Algorithms;
using JWT.Builder;

namespace BaseAuth.Manager;

/// <summary>
/// An enum representing the type of token (access or refresh)
/// </summary>
public enum TokenType
{
    Access,
    Refresh
}

/// <summary>
/// A class representing a token with an access token, a refresh token, and their respective expiration times
/// </summary>
/// <seealso cref="TokenType"/>/>
public class Token
{
    /// <summary>
    /// The lifetime of the access token in seconds (default 1 hour)
    /// </summary>
    [JsonIgnore] public const int AccessTokenLifetime = 1 * 60 * 60;

    /// <summary>
    /// The lifetime of the refresh token in seconds (default 7 days)
    /// </summary>
    [JsonIgnore] public const int RefreshTokenLifetime = 7 * 24 * 60 * 60;

    /// <summary>
    /// The access token to be used for authentication
    /// </summary>
    public string AccessToken { get; set; } = "";

    /// <summary>
    /// The refresh token to be used to get a new access token
    /// </summary>
    public string RefreshToken { get; set; } = "";

    /// <summary>
    /// The time at which the access token expires
    /// </summary>
    public DateTime AccessExpiresAt { get; set; }

    /// <summary>
    /// The time at which the refresh token expires
    /// </summary>
    public DateTime RefreshExpiresAt { get; set; }
}

public class TokenManager
{
    /// <summary>
    /// A list of all tokens that have been generated
    /// </summary>
    public static List<Token> ListTokens = [];

    /// <summary>
    /// Generates a new token with the given claims
    /// </summary>
    /// <param name="claims">
    /// The claims to be added to the token (e.g. user id, role, etc.)
    /// </param>
    /// <returns>
    /// A new token with the given claims
    /// </returns>
    /// <seealso cref="Token"/>
    public static Token GenerateToken(IEnumerable<KeyValuePair<string, object>> claims)
    {
        var accessToken = GenerateJwt(claims, TokenType.Access);
        var refreshToken = GenerateJwt(claims, TokenType.Refresh);

        lock (ListTokens)
        {
            var token = new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessExpiresAt = DateTimeOffset.UtcNow.ToLocalTime().AddSeconds(Token.AccessTokenLifetime).DateTime,
                RefreshExpiresAt = DateTimeOffset.UtcNow.ToLocalTime().AddSeconds(Token.RefreshTokenLifetime).DateTime
            };

            ListTokens.Add(token);
            return token;
        }
    }
    
    /// <summary>
    /// Generates a new JWT token with the given claims and lifetime
    /// </summary>
    /// <param name="claims">
    /// The claims to be added to the token
    /// </param>
    /// <param name="type">
    /// The type of token to be generated (access or refresh)
    /// </param>
    /// <param name="lifetime">
    /// The lifetime of the token in seconds
    /// </param>
    /// <returns>
    /// A new JWT token with the given claims and lifetime
    /// </returns>
    private static string GenerateJwt(IEnumerable<KeyValuePair<string, object>> claims, TokenType type, int lifetime)
    {
        var expiresAt = DateTimeOffset.UtcNow.ToLocalTime().AddSeconds(lifetime).ToUnixTimeSeconds();

        var builder = new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret("secret")
            .AddClaim("type", type)
            .AddClaim("expiredAt", expiresAt)
            .AddClaim("expiredIn", lifetime);

        if (type == TokenType.Access)
        {
            builder.AddClaims(claims);
        }

        return builder.Encode();
    }

    /// <summary>
    /// Generates a new JWT token with the given claims with default lifetime
    /// </summary>
    /// <param name="claims">
    /// The claims to be added to the token
    /// </param>
    /// <param name="type">
    /// The type of token to be generated (access or refresh)
    /// </param>
    /// <returns>
    /// A new JWT token with the given claims
    /// </returns>
    /// <seealso cref="GenerateToken"/>
    /// <seealso cref="TokenType"/>
    /// <seealso cref="GenerateJwt(System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{string,object}},BaseAuth.Manager.TokenType,int)"/>
    private static string GenerateJwt(IEnumerable<KeyValuePair<string, object>> claims, TokenType type)
    {
        var lifetime = type == TokenType.Access ? Token.AccessTokenLifetime : Token.RefreshTokenLifetime;
        return GenerateJwt(claims, type, lifetime);
    }

    /// <summary>
    /// Decodes the given JWT token and returns the claims
    /// </summary>
    /// <param name="token"></param>
    /// <returns>
    /// A dictionary of claims from the given token
    /// </returns>
    /// <seealso cref="Token"/>
    private static IDictionary<string, object> Claims(string token)
    {
        return new JwtBuilder()
            .WithSecret("secret")
            .MustVerifySignature()
            .Decode<IDictionary<string, object>>(token);
    }

    /// <summary>
    /// Validates the given token and checks if it is still valid
    /// </summary>
    /// <param name="token">
    /// The token to be validated
    /// </param>
    /// <param name="type">
    /// The type of token to be validated (access or refresh)
    /// </param>
    /// <returns>
    /// True if the token is valid and has not expired, false otherwise
    /// </returns>
    /// <seealso cref="Token"/>
    /// <seealso cref="TokenType"/>
    public static bool ValidateToken(string token, TokenType type)
    {
        try
        {
            var jwt = Claims(token);

            if (jwt["type"].ToString() != type.ToString())
                return false;

            var expiresAt = long.Parse(jwt["exp"].ToString());
            var isExpired = DateTimeOffset.UtcNow.ToUnixTimeSeconds() < expiresAt;

            if (jwt["type"].ToString() == TokenType.Refresh.ToString() && isExpired)
            {
                RevokeToken(token);
                return false;
            }

            return !isExpired;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    /// <summary>
    /// Revokes the given token and removes it from the list of tokens
    /// </summary>
    /// <param name="token">
    /// The token to be revoked
    /// </param>
    /// <seealso cref="Token"/>
    public static void RevokeToken(string token)
    {
        lock (ListTokens)
        {
            var index = ListTokens.FindIndex(x => x.AccessToken == token || x.RefreshToken == token);
            if (index == -1)
                return;

            ListTokens.RemoveAt(index);
        }
    }

    /// <summary>
    /// Revokes all tokens and clears the list of tokens
    /// </summary>
    /// <seealso cref="Token"/>
    public static void RevokeAllTokens()
    {
        lock (ListTokens)
        {
            ListTokens.Clear();
        }
    }

    /// <summary>
    /// Refreshes the given refresh token and returns a new token
    /// </summary>
    /// <param name="refreshToken">
    /// The refresh token to be refreshed
    /// </param>
    /// <returns>
    /// A new token if the refresh token is valid, null otherwise
    /// </returns>
    public static Token? RefreshToken(string refreshToken)
    {
        lock (ListTokens)
        {
            // Find the token in the list
            var index = ListTokens.FindIndex(x => x.RefreshToken == refreshToken);
            if (index == -1)
                return null;

            // Get the token
            var token = ListTokens[index];

            // Check if the token is still valid
            if (!ValidateToken(token.RefreshToken, TokenType.Refresh))
                return null;
            
            //-- Generate a new access token --//
            // Claim the access token
            var accessClaims = Claims(token.AccessToken);

            // Claim user info from the old access token
            var userInfo = new Dictionary<string, object>();
            foreach (var (key, value) in accessClaims)
            {
                if (key != "type" && key != "expiredAt" && key != "expiredIn")
                    userInfo.Add(key, value);
            }
            var userClaims = userInfo.ToList();

            var newAccessToken = GenerateJwt(userClaims, TokenType.Access);
            
            //-- Generate a new refresh token --//
            // Get the refresh token claims
            var refreshClaims = Claims(token.RefreshToken);
            
            // Calculate the remaining lifetime of the refresh token
            var remainingLifetime = long.Parse(refreshClaims["expiredAt"].ToString()) - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            // Generate a new refresh token with the remaining lifetime
            var newRefreshToken = GenerateJwt(userClaims, TokenType.Refresh, (int) remainingLifetime);
            
            // Update the token in the list
            ListTokens[index] = new Token
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessExpiresAt = DateTimeOffset.UtcNow.ToLocalTime().AddSeconds(Token.AccessTokenLifetime).DateTime,
                RefreshExpiresAt = DateTimeOffset.UtcNow.ToLocalTime().AddSeconds(remainingLifetime).DateTime
            };
            
            return ListTokens[index];
        }
    }
}