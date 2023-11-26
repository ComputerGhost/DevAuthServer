namespace DevAuthServer.Constants;

public enum FlowType
{
    Invalid,

    /// <summary>
    /// The `authorization` endpoint returns Authorization Codes.
    /// The `token` endpoint returns tokens.
    /// </summary>
    AuthorizationCode,

    /// <summary>
    /// The `authorization` endpoint returns Authorization Codes and some tokens.
    /// The `token` endpoint returns some tokens.
    /// </summary>
    Hybrid,

    /// <summary>
    /// Only the `authorization` endpoint is used, and it returns the tokens.
    /// </summary>
    Implicit,
}
