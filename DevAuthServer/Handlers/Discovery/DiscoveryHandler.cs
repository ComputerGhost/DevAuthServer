using DevAuthServer.Constants;

namespace DevAuthServer.Handlers.Discovery;

public class DiscoveryHandler
{
    public DiscoveryOutputModel Process()
    {
        var baseUrl = OIDCConfig.Instance.Issuer;

        return new DiscoveryOutputModel
        {
            issuer = baseUrl,
            authorization_endpoint = $"{baseUrl}/authorize",
            token_endpoint = $"{baseUrl}/token",
            userinfo_endpoint = $"{baseUrl}/userinfo",
            jwks_uri = "${baseUrl}/jwks",
            response_types_supported = new[] { "code", "id_token", "id_token token", "token", "code token", "code id_token", "code id_token token" },
            grant_types_supported = new[] { "authorization_code", "client_credentials", "implicit", "password", "refresh_token" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "HS256", "RS256" },
            token_endpoint_auth_methods_supported = new[] { "client_secret_basic", "client_secret_post" },

            check_session_iframe = $"{baseUrl}/check-session",
            end_session_endpoint = $"{baseUrl}/end-session",
            introspection_endpoint = $"{baseUrl}/introspect",
        };
    }
}
