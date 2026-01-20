using System;
using System.IO;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register SecretClient if configured
var kvUri = builder.Configuration.GetValue<string>("KeyVault:VaultUri");
if (!string.IsNullOrEmpty(kvUri))
{
    var credential = new DefaultAzureCredential();
    var secretClient = new SecretClient(new Uri(kvUri), credential);
    builder.Services.AddSingleton(secretClient);
    // Add an Azure-specific service implementation in this project
    builder.Services.AddSingleton<ISecretsManagerService, AzureKeyVaultSecretsService>();
}

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
