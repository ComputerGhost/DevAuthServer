using DevAuthServer.Constants;
using DevAuthServer.Storage;

var builder = WebApplication.CreateBuilder(args);

OIDCConfig.Instance = builder.Configuration.GetSection("OIDC").Get<OIDCConfig>()!;

builder.Services.AddMvc();
builder.Services.AddSingleton<Database>();

var app = builder.Build();
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());
app.MapControllers();

await app.RunAsync();
