using DevAuthServer.Constants;
using DevAuthServer.Storage;

var builder = WebApplication.CreateBuilder(args);

OIDCConfig.Instance = builder.Configuration.GetSection("OIDCConfig").Get<OIDCConfig>()!;

builder.Services.AddMvc();
builder.Services.AddSingleton<Database>();

var app = builder.Build();
app.MapControllers();

await app.RunAsync();
