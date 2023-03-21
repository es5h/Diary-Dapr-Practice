using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using NotificationApi.Command;
using NotificationApi.Events;

namespace NotificationApi.Controllers;

[ApiController]
public class NotificationController : ControllerBase
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(DaprClient daprClient, ILogger<NotificationController> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    [Route("sendmail")]
    [HttpPost]
    [Topic("eventbus", "DiaryOrderProcessedEvent")]
    public async Task<IActionResult> SendEmail(DispatchedOrderCommand command)
    {
        _logger.LogInformation("sendmail entered for {DiaryId}", command.DiaryId);

        var metadata = new Dictionary<string, string>
        {
            { "emailFrom", "noreply@abc.com" },
            { "emailTo", command.UserEmail ?? string.Empty },
            { "subject", $"{command.Title} is ready for you" },
            { "body", command.GeneratedContent ?? string.Empty }
        };

        var body = CreateEmailBody(command);
        await _daprClient.InvokeBindingAsync("email", "create", body, metadata);

        await _daprClient.PublishEventAsync("eventbus", "OrderDispatchedEvent",
            new OrderDispatchedEvent(command.DiaryId, DateTime.UtcNow));

        _logger.LogInformation("order {DiaryId} dispatched", command.DiaryId);
        return Ok();
    }

    private static string CreateEmailBody(DispatchedOrderCommand command)
    {
        return $@"
            <b>
                {command.GeneratedContent}
            </b>";
    }
}