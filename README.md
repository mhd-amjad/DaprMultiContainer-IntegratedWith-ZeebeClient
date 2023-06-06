Complete multi containered solution contains 2 projects Using Dapr service invocation building block and integrated with Zeebe engine client.

- MyBackend Project:
  It is a simple API has a single method that returns weather forecast.
  
- MyFrontend project:
  A web application that calls the MyBackends method and gets weather information using Dapr service invocation technique and shows the results in the browser.
  And this task is integrated with Task service and being modeled in Camunda modeler using zeebe engine client.
  
  
 Project pre-requisities:
 - .net 7 SDK.
 - Docker CLI.
 - Dapr CLI.
 - create account on Camunda as well a cluster & client then replacing the credentials in the MyFrontend/CamundaCloud.env file.
