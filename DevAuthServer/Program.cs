using DevAuthServer.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();
app.MapRazorPages();
app.MapEndpoints();

await app.RunAsync();
