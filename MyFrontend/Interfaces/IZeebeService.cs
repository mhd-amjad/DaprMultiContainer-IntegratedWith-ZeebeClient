using Zeebe.Client.Api.Responses;

namespace MyFrontend;

public interface IZeebeService
{
    public Task<IDeployResponse> Deploy(string modelFilename);
    public Task<ITopology> Status();
    public Task<String> StartWorkflowInstance(string bpmProcessId);
    public void CreateGetWeatherWorker();
}