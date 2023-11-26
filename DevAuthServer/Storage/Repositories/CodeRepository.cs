using DevAuthServer.Storage.Entities;

namespace DevAuthServer.Storage.Repositories;

public class CodeRepository
{
    private readonly Dictionary<string, CodeEntity> _byCode = new();

    public CodeEntity CreateCode()
    {
        var entity = new CodeEntity(Guid.NewGuid().ToString());
        _byCode.Add(entity.Code, entity);
        return entity;
    }

    public void Delete(string code)
    {
        _byCode.Remove(code);
    }

    public CodeEntity? GetCode(string code)
    {
        return _byCode.TryGetValue(code, out CodeEntity? codeEntity) ? codeEntity : null;
    }
}
