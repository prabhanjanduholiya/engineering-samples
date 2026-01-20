using System.Text.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

public sealed class SecretsManagerService : ISecretsManagerService
{
    private readonly IAmazonSecretsManager _client;

    public SecretsManagerService(IAmazonSecretsManager client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task ExecuteAsync(string command, string[] args)
    {
        // Maintain backward compatibility with simple console-style commands
        switch (command)
        {
            case "list":
                await ListSecrets();
                break;
            case "get":
                if (args.Length < 2) { Console.WriteLine("Missing secret name"); return; }
                await GetSecret(args[1]);
                break;
            case "create":
                if (args.Length < 3) { Console.WriteLine("Missing name/value"); return; }
                await CreateSecretIfNotExists(args[1], args[2]);
                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }
    }

    public async Task<string[]> ListSecretsAsync()
    {
        var request = new ListSecretsRequest { MaxResults = 50 };
        var response = await _client.ListSecretsAsync(request);
        var names = response.SecretList.Select(s => s.Name).ToArray();
        return names!;
    }

    public async Task<string?> GetSecretAsync(string name)
    {
        var req = new GetSecretValueRequest { SecretId = name };
        var res = await _client.GetSecretValueAsync(req);
        if (res.SecretString != null)
        {
            return res.SecretString;
        }
        else if (res.SecretBinary != null)
        {
            return Convert.ToBase64String(res.SecretBinary.ToArray());
        }

        return null;
    }

    public async Task CreateSecretIfNotExistsAsync(string name, string value)
    {
        try
        {
            var createReq = new CreateSecretRequest { Name = name, SecretString = value };
            var createRes = await _client.CreateSecretAsync(createReq);
        }
        catch (ResourceExistsException)
        {
            // ignore - resource exists
        }
    }

    // Keep original internal helpers used by ExecuteAsync for console compatibility
    private async Task ListSecrets()
    {
        Console.WriteLine("Listing secrets (first page):");
        var request = new ListSecretsRequest { MaxResults = 50 };
        var response = await _client.ListSecretsAsync(request);
        foreach (var s in response.SecretList)
        {
            Console.WriteLine($"- {s.Name} (ARN: {s.ARN})");
        }
    }

    private async Task GetSecret(string secretName)
    {
        Console.WriteLine($"Getting secret: {secretName}");
        var req = new GetSecretValueRequest { SecretId = secretName };
        var res = await _client.GetSecretValueAsync(req);
        if (res.SecretString != null)
        {
            try
            {
                using var doc = JsonDocument.Parse(res.SecretString);
                Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (JsonException)
            {
                Console.WriteLine(res.SecretString);
            }
        }
        else if (res.SecretBinary != null)
        {
            Console.WriteLine($"Binary secret ({res.SecretBinary.Length} bytes)");
        }
    }

    private async Task CreateSecretIfNotExists(string name, string value)
    {
        Console.WriteLine($"Creating secret: {name}");
        try
        {
            var createReq = new CreateSecretRequest { Name = name, SecretString = value };
            var createRes = await _client.CreateSecretAsync(createReq);
            Console.WriteLine($"Created: {createRes.ARN}");
        }
        catch (ResourceExistsException)
        {
            Console.WriteLine("Secret already exists. Fetching current value...");
            await GetSecret(name);
        }
    }
}
