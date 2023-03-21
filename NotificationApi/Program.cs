using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddDapr();

var app = builder.Build();

app.UseCloudEvents();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapSubscribeHandler();

app.Run();
