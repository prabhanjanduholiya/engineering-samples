using Azure.Security.KeyVault.Secrets;

public sealed class AzureKeyVaultSecretsService : ISecretsManagerService
{
    private readonly SecretClient _client;

    public AzureKeyVaultSecretsService(SecretClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public Task ExecuteAsync(string command, string[] args) => throw new NotImplementedException();

    public async Task<string[]> ListSecretsAsync()
    {
        var names = new List<string>();
        await foreach (var prop in _client.GetPropertiesOfSecretsAsync())
        {
            if (!string.IsNullOrEmpty(prop.Name)) names.Add(prop.Name);
        }
        return names.ToArray();
    }

    public async Task<string?> GetSecretAsync(string name)
    {
        try
        {
            var res = await _client.GetSecretAsync(name);
            return res.Value.Value;
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task CreateSecretIfNotExistsAsync(string name, string value)
    {
        try
        {
            var existing = await _client.GetSecretAsync(name);
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            await _client.SetSecretAsync(name, value);
        }
    }
}
