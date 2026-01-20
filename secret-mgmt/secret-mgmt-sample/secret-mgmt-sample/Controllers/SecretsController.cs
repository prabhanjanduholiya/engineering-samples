using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/aws/secret-manager/secrets")]
public class SecretsController : ControllerBase
{
    private readonly ISecretsManagerService _service;

    public SecretsController(ISecretsManagerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var list = await _service.ListSecretsAsync();
        return Ok(list);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> Get(string name)
    {
        var secret = await _service.GetSecretAsync(name);
        if (secret == null) return NotFound();
        return Ok(secret);
    }

    [HttpPost("{name}")]
    public async Task<IActionResult> Create(string name, [FromBody] string value)
    {
        await _service.CreateSecretIfNotExistsAsync(name, value);
        return CreatedAtAction(nameof(Get), new { name }, null);
    }
}
