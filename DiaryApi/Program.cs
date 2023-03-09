using DiaryApi;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new OpenAiServiceConfiguration();
builder.Configuration.Bind("OpenAIServiceOptions", config);
builder.Services.AddSingleton(config);

builder.Services.AddControllers().AddDapr();

builder.Services.AddOpenAIService(settings =>
{
    settings.ApiKey = config.ApiKey;
});
builder.Services.AddSingleton<IOpenAIService, OpenAIService>();

var app = builder.Build();

app.UseCloudEvents();
app.UseAuthorization();

app.MapControllers();
app.MapSubscribeHandler();

app.Run();