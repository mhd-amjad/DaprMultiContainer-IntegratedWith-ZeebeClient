using Zeebe.Client.Api.Responses;
using Zeebe.Client.Impl.Builder;
using Zeebe.Client;
using fastJSON;
using Zeebe.Client.Api.Worker;

namespace MyFrontend;

public class ZeebeService : IZeebeService
{
    private readonly IZeebeClient _client;

    public ZeebeService(ILogger<ZeebeService> logger)
    {
        var authServer = Environment.GetEnvironmentVariable("ZEEBE_AUTHORIZATION_SERVER_URL");
        var clientId = Environment.GetEnvironmentVariable("ZEEBE_CLIENT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("ZEEBE_CLIENT_SECRET");
        var zeebeUrl = Environment.GetEnvironmentVariable("ZEEBE_ADDRESS");
        char[] port =
        {
            '4', '3', ':'
        };
        var audience = zeebeUrl?.TrimEnd(port);

        _client =
            ZeebeClient.Builder()
                .UseGatewayAddress(zeebeUrl)
                .UseTransportEncryption()
                .UseAccessTokenSupplier(
                    CamundaCloudTokenProvider.Builder()
                        .UseAuthServer(authServer)
                        .UseClientId(clientId)
                        .UseClientSecret(clientSecret)
                        .UseAudience(audience)
                        .Build())
                .Build();
    }

    public Task<ITopology> Status()
    {
        return _client.TopologyRequest().Send();
    }

    public async Task<IDeployResponse> Deploy(string modelFilename)
    {
        var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Resources", modelFilename);
        var deployment = await _client.NewDeployCommand().AddResourceFile(filename).Send();
        return deployment;
    }

    public async Task<String> StartWorkflowInstance(string bpmProcessId)
    {
        var instance = await _client.NewCreateProcessInstanceCommand()
                .BpmnProcessId(bpmProcessId)
                .LatestVersion()
                .WithResult()
                .Send();
        var jsonParams = new JSONParameters { ShowReadOnlyProperties = true };
        return JSON.ToJSON(instance, jsonParams);
    }

    public void CreateGetWeatherWorker()
    {
        _createWorker("get-weather", async (client, job) =>
        {
            await client.NewCompleteJobCommand(job.Key).Send();
        });
    }

    private void _createWorker(String jobType, JobHandler handleJob)
    {
        _client.NewWorker()
                .JobType(jobType)
                .Handler(handleJob)
                .MaxJobsActive(5)
                .Name(jobType)
                .PollInterval(TimeSpan.FromSeconds(50))
                .PollingTimeout(TimeSpan.FromSeconds(50))
                .Timeout(TimeSpan.FromSeconds(10))
                .Open();
    }
}