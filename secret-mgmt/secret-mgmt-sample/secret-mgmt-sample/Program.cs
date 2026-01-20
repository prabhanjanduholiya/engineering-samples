using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add configuration sources
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register AWS Secrets Manager client using region from configuration (or default)
var awsRegion = builder.Configuration.GetValue<string>("AWS:Region") ?? "us-east-1";
var awsConfig = new AmazonSecretsManagerConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion) };
var awsClient = new AmazonSecretsManagerClient(awsConfig);

builder.Services.AddSingleton<IAmazonSecretsManager>(awsClient);
builder.Services.AddSingleton<ISecretsManagerService, SecretsManagerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
