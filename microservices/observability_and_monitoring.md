# Health Monitoring
Health monitoring can allow near-real-time information about the state of your containers and microservices.

## Implement health checks in ASP.NET Core services
https://blog.zhaytam.com/2020/04/30/health-checks-aspnetcore/
### Health check service as middleware
* Health check services and middleware are easy to use and provide capabilities that let you validate if any external resource needed for your application (like a SQL Server database or a remote API) is working properly. 
* Asp.Net core provides built-in healthcheck feature through Nuget `Microsoft.Extensions.Diagnostics.HealthChecks`. Install this nuget if it is not there.

### Use the HealthChecks feature in your back-end ASP.NET microservices
