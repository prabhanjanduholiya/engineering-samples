public interface ISecretsManagerService
{
    Task ExecuteAsync(string command, string[] args);
    Task<string[]> ListSecretsAsync();
    Task<string?> GetSecretAsync(string name);
    Task CreateSecretIfNotExistsAsync(string name, string value);
}
