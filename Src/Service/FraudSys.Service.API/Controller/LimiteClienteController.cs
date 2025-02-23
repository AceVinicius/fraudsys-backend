namespace FraudSys.Service.API.Controller;

[ApiController]
[Route("api/limiteCliente")]
public class LimiteClienteController : ControllerBase
{
    private readonly IAppLogger<LimiteClienteController> _appLogger;
    private readonly ICadastrarLimiteUseCase _cadastrarLimiteUseCase;
    private readonly IAtualizarLimiteUseCase _atualizarLimiteUseCase;
    private readonly IRemoverLimiteUseCase _removerLimiteUseCase;
    private readonly IBuscarLimiteUseCase _buscarLimiteUseCase;
    private readonly IBuscarTodosLimitesUseCase _buscarTodosLimitesUseCase;

    public LimiteClienteController(
        IAppLogger<LimiteClienteController> appLogger,
        ICadastrarLimiteUseCase cadastrarLimiteUseCase,
        IAtualizarLimiteUseCase atualizarLimiteUseCase,
        IRemoverLimiteUseCase removerLimiteUseCase,
        IBuscarLimiteUseCase buscarLimiteUseCase,
        IBuscarTodosLimitesUseCase buscarTodosLimitesUseCase)
    {
        _appLogger = appLogger;
        _cadastrarLimiteUseCase = cadastrarLimiteUseCase;
        _atualizarLimiteUseCase = atualizarLimiteUseCase;
        _removerLimiteUseCase = removerLimiteUseCase;
        _buscarLimiteUseCase = buscarLimiteUseCase;
        _buscarTodosLimitesUseCase = buscarTodosLimitesUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CadastrarLimiteOutput), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Cadastrar(
        [FromBody] CadastrarLimiteInput input,
        CancellationToken cancellationToken)
    {
        var output = await _cadastrarLimiteUseCase.Execute(input, cancellationToken);

        return Ok(output);
    }

    [HttpPatch("{documento}")]
    [ProducesResponseType(typeof(AtualizarLimiteOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        [FromRoute] string documento,
        [FromBody] AtualizarRequest novoLimite,
        CancellationToken cancellationToken)
    {
        var output = await _atualizarLimiteUseCase.Execute(
            new AtualizarLimiteInput(documento, novoLimite.NovoLimite),
            cancellationToken);

        return Ok(output);
    }

    [HttpDelete("{documento}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(
        [FromRoute] string documento,
        CancellationToken cancellationToken)
    {
        _ = await _removerLimiteUseCase.Execute(
            new RemoverLimiteInput(documento),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{documento}")]
    [ProducesResponseType(typeof(BuscarLimiteOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarPorId(
        [FromRoute] string documento,
        CancellationToken cancellationToken)
    {
        var output = await _buscarLimiteUseCase.Execute(
            new BuscarLimiteInput(documento),
            cancellationToken);

        return Ok(output);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BuscarTodosLimitesOutput>), StatusCodes.Status200OK)]
    public async Task<IActionResult> BuscarTodos(
        CancellationToken cancellationToken)
    {
        var output = await _buscarTodosLimitesUseCase.Execute(
            new BuscarTodosLimitesInput(),
            cancellationToken);

        return Ok(output);
    }
}

public class AtualizarRequest(decimal novoLimite)
{
    public decimal NovoLimite { get; } = novoLimite;
}