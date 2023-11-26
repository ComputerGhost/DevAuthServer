namespace DevAuthServer.Storage.Entities;

public class ClientEntity
{
    public string ClientId { get; set; } = null!;
    public IEnumerable<string> RedirectUris { get; set; } = null!;
}
