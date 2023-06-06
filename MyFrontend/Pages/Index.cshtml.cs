using Dapr.Client;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly DaprClient _daprClient;
    private readonly IZeebeService _zeebeService;

    public IndexModel(DaprClient daprClient, IZeebeService zeebeService)
    {
        _daprClient = daprClient;
        _zeebeService = zeebeService;
    }

    public async Task OnGet()
    {
        await _zeebeService.Deploy("test-process.bpmn");
        _zeebeService.CreateGetWeatherWorker();
        //var temp = await _zeebeService.Status();
        var workFlow = _zeebeService.StartWorkflowInstance("test-process");

        var forecasts = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(
            HttpMethod.Get,
            "MyBackend",
            "weatherforecast");

        ViewData["WeatherForecastData"] = forecasts;
    }
}