using DevAuthServer.Storage;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddSingleton<Database>();

var app = builder.Build();
app.MapControllers();

await app.RunAsync();
