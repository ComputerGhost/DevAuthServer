namespace DevAuthServer.Storage.Entities;

public class CodeEntity
{
    public CodeEntity(string code)
    {
        Code = code;
    }

    public string Code { get; set; } = null!;
}
