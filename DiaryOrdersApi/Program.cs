using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OrdersApi;
using OrdersApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IConfig>(builder.Configuration.GetSection("CustomConfig")?.Get<Config>() ?? throw new InvalidOperationException());
builder.Services.AddDbContext<DiaryOrdersContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DiaryOrderConnection") ?? "name=DiaryOrderConnection", opt =>
            opt.EnableRetryOnFailure(5)
    );
}, ServiceLifetime.Transient);

builder.Services.AddTransient<IDiaryOrderRepository, DiaryOrderRepository>();
builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

app.UseCloudEvents();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapSubscribeHandler();

// migration databse
var config = app.Services.GetService<IConfig>();
if (config?.RunDbMigrations == true)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DiaryOrdersContext>();
    context.Database.Migrate();
}

app.Run();