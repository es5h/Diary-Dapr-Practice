using Microsoft.EntityFrameworkCore;
using OrdersApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DiaryOrdersContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DiaryOrderConnection")
));

builder.Services.AddTransient<IDiaryOrderRepository, DiaryOrderRepository>();
builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

app.UseCloudEvents();
app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapSubscribeHandler();

app.Run();