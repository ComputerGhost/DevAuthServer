﻿using DevAuthServer.Constants;
using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Token;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace DevAuthServer.Storage.Entities;

public class IdToken
{
    public IdToken(AuthorizeInputModel input, User user)
        : this(user, input.client_id, input.nonce)
    { }

    public IdToken(TokenInputModel input, User user, AuthorizationCode code)
        : this(user, input.client_id, code.Nonce)
    { }

    public IdToken(TokenInputModel input, User user)
        : this(user, input.client_id, null)
    { }

    private IdToken(User user, string clientId, string? nonce)
    {
        // Core claims
        iss = OIDCConfig.Instance.Issuer;
        sub = user.Id;
        aud = clientId;
        exp = OIDCConfig.Instance.TokenExpiration;
        iat = new DateTime().Ticks / 1000; // Time of token creation
        auth_time = user.LoginTime;
        this.nonce = nonce;

        // Additional claims... return everything regardless of scope.
        email = user.Email;
        picture = user.PictureUrl;
        preferred_username = user.DisplayName;
        roles = user.Roles?.Split(' ');
    }

    // OIDC Core claims from <https://openid.net/specs/openid-connect-core-1_0.html>
    public string iss { get; set; } = null!;
    public string sub { get; set; } = null!;
    public string aud { get; set; } = null!;
    public long exp { get; set; }
    public long iat { get; set; }
    public long auth_time { get; set; }
    public string? nonce { get; set; }

    // Common additional claims from <https://www.iana.org/assignments/jwt/jwt.xhtml>.
    public string email { get; set; } = null!;
    public bool email_verified { get; set; } = true;
    public string? picture { get; set; }
    public string? preferred_username { get; set; }
    public IEnumerable<string>? roles { get; set; }

    public string Encode()
    {
        var header = "{\"typ\": \"JWT\", \"alg\": \"HS256\"}";
        var encodedHeader = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(header));

        var payload = JsonConvert.SerializeObject(this);
        var encodedPayload = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(payload));

        var secret = Encoding.UTF8.GetBytes(OIDCConfig.Instance.ClientSecret);
        var source = Encoding.UTF8.GetBytes(encodedPayload);
        var signature = HMACSHA256.HashData(secret, source);
        var encodedSignature = WebEncoders.Base64UrlEncode(signature);

        return $"{encodedHeader}.{encodedPayload}.{encodedSignature}";
    }
}
