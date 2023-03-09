using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using MvcFront.Commands;
using OrdersApi.Events;
using OrdersApi.Models;
using OrdersApi.Persistence;

namespace OrdersApi.Controllers;

[ApiController]
public class DiaryOrdersController:ControllerBase
{
    private readonly ILogger<DiaryOrdersController> _logger;
    private readonly IDiaryOrderRepository _diaryOrderRepository;
    private readonly DaprClient _daprClient;

    public DiaryOrdersController(ILogger<DiaryOrdersController> logger, IDiaryOrderRepository diaryOrderRepository, DaprClient daprClient)
    {
        _logger = logger;
        _diaryOrderRepository = diaryOrderRepository;
        _daprClient = daprClient;
    }
    
    [Route("DiaryItemReceived")]
    [HttpPost]
    [Topic("eventbus", "DiaryItemReceivedEvent")]
    public async Task<IActionResult> DiaryItemReceived([FromBody] DiaryOrderReceivedCommand command)
    {
        if (command?.DiaryId is null || command?.Title is null || command?.UserEmail is null ||
            command?.ContentItem is null)
        {
            return BadRequest();
        }

        _logger.LogInformation("DiaryItemReceivedEvent received");
        
        var orderExists = await _diaryOrderRepository.GetDiaryOrderAsync(command.DiaryId)!;

        if (orderExists is null)
        {
            await _diaryOrderRepository.RegisterDiaryOrder(new DiaryOrder
            {
                DiaryId = command.DiaryId,
                Title = command.Title,
                UserEmail = command.UserEmail,
                ContentItem = command.ContentItem,
                FeelingScore = command.FeelingScore,
                CreatedAt = DateTime.Now,
                Status = Status.Registered,
            });
            
            await _daprClient.PublishEventAsync("eventbus", "DiaryOrderRegisteredEvent", new DiaryOrderRegisteredEvent()
            { 
                DiaryId = command.DiaryId,
                Title = command.Title,
                UserEmail = command.UserEmail,
            });
        }

        _logger.LogInformation("DiaryItemReceivedEvent published");
        return Ok();
    }
}