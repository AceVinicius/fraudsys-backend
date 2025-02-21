using FraudSys.Application.Command.EfetuarTransacao;

namespace FraudSys.Service.API.Controller;

[ApiController]
[Route("transacao")]
public class TransacaoController : ControllerBase
{
    private readonly IAppLogger<LimiteClienteController> _appLogger;
    private readonly IEfetuarTransacaoUseCase _efetuarTransacaoUseCase;

    public TransacaoController(
        IAppLogger<LimiteClienteController> appLogger,
        IEfetuarTransacaoUseCase efetuarTransacaoUseCase)
    {
        _appLogger = appLogger;
        _efetuarTransacaoUseCase = efetuarTransacaoUseCase;
    }

    [HttpPost("store")]
    [ProducesResponseType(typeof(EfetuarTransacaoOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cadastrar(
        [FromBody] EfetuarTransacaoInput input,
        CancellationToken cancellationToken)
    {
        var output = await _efetuarTransacaoUseCase.Execute(input, cancellationToken);

        return Ok(output);
    }
}