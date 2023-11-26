using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage.Repositories;

public class ClientRepository
{
    private readonly Dictionary<string, ClientEntity> _byClientId = new();

    public void AddClient(ClientEntity client)
    {
        _byClientId.Add(client.ClientId, client);
    }

    public ClientEntity? GetClient(string clientId)
    {
        return _byClientId.TryGetValue(clientId, out ClientEntity? client)? client : null;
    }
}
