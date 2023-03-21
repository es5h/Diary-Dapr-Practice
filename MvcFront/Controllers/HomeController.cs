using System.Diagnostics;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using MvcFront.Events;
using MvcFront.Models;

namespace MvcFront.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DaprClient _daprClient;

    public HomeController(ILogger<HomeController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }

    [HttpGet]
    public IActionResult WriteDiary()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> WriteDiary(WriteDiaryCommand command)
    {
        var eventData = new DiaryItemReceivedEvent(Guid.NewGuid(), command.Title, command.UserEmail,
            command.Content?.Split("-"), command.FeelingScore, DateTime.Now);

        try
        {
            await _daprClient.PublishEventAsync("eventbus", "DiaryItemReceivedEvent", eventData);
            _logger.LogInformation("Published event: DiaryItemReceivedEvent, Diary Id : {diaryId}", eventData.DiaryId);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error publishing event: DiaryItemReceivedEvent, Diary Id : {diaryId} Error: {error}",
                eventData.DiaryId, e.Message);
            throw;
        }

        ViewData["DiaryId"] = eventData.DiaryId;

        return View("Thanks");
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}