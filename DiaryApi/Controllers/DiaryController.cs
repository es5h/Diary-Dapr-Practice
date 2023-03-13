using Dapr;
using Dapr.Client;
using DiaryApi.Commands;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace DiaryApi.Controllers;

[ApiController]
public class DiaryController : ControllerBase
{
    private readonly ILogger<DiaryController> _logger;
    private readonly DaprClient _daprClient;
    private readonly IOpenAIService _openAiService;
    private readonly OpenAiServiceConfiguration _openAiServiceConfiguration;

    public DiaryController(ILogger<DiaryController> logger, DaprClient daprClient, IOpenAIService openAiService, OpenAiServiceConfiguration openAiServiceConfiguration)
    {
        _logger = logger;
        _daprClient = daprClient;
        _openAiService = openAiService;
        _openAiServiceConfiguration = openAiServiceConfiguration;
    }

    [Route("ProcessOrder")]
    [HttpPost]
    [Topic("eventbus", "DiaryOrderRegisteredEvent")]
    public async Task<IActionResult> ProcessOrder([FromBody] ProcessOrderCommand? command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        if (command?.DiaryId is null || command.Title is null || command.UserEmail is null)
        {
            return BadRequest();
        }
        
        var ordersState = await _daprClient.GetStateEntryAsync<List<ProcessOrderCommand>>("redisstore", "orders");
        var orders = ordersState.Value ?? new List<ProcessOrderCommand>();
        orders.Add(command);
        await _daprClient.SaveStateAsync("redisstore", "orders", orders);

        return Ok();
    }

    [HttpPost("cron")]
    public async Task<IActionResult> Cron()
    {
        _logger.LogInformation("Cron job started");
        var ordersState = await _daprClient.GetStateEntryAsync<List<ProcessOrderCommand>>("redisstore", "orders");
        
        if (ordersState.Value is null || ordersState.Value.Count == 0)
        {
            _logger.LogInformation("No orders to process");
            return NoContent();
        }
        
        _logger.LogInformation("the count of orders is {count}", ordersState.Value.Count);
        
        var firstOrder = ordersState.Value.First();
        
        var generatedContent = await GenerateContentAsync(firstOrder.ContentItem, firstOrder.FeelingScore);
        await _daprClient.PublishEventAsync("eventbus", "DiaryOrderProcessedEvent",
            new DiaryOrderProcessedEvent(firstOrder.DiaryId, firstOrder.Title, firstOrder.UserEmail, generatedContent));

        ordersState.Value.Remove(firstOrder);

        await _daprClient.SaveStateAsync("redisstore", "orders", ordersState.Value);
        
        _logger.LogInformation("Cron job finished and the count of orders is {count}", ordersState.Value.Count);

        return Ok();
    }

    private async Task<string?> GenerateContentAsync(IEnumerable<string?>? contentItem, int feelingScore)
    {
        var completionResult = await _openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
        {
            Prompt = _openAiServiceConfiguration.Prompt
                .Replace("{contentItems}", string.Join(",", contentItem ?? new List<string?>()))
                .Replace("{feelingScore}", feelingScore.ToString()),
            Model = Models.TextDavinciV3
        });

        return completionResult.Successful ? completionResult.Choices.FirstOrDefault()?.Text : "Failed";
    }
    
    [HttpGet]
    [Route("/")]
    public async Task<IActionResult> Get()
    {
        var generatedContent = await GenerateContentAsync(new List<string?> {"test 하기", "밥 먹기"}, 10);
        return Ok(generatedContent);
    }
}