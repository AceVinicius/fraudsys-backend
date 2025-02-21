namespace FraudSys.Service.API.Controller;

[ApiController]
[Route("limiteCliente")]
public class LimiteClienteController : ControllerBase
{
    private readonly Application.Repository.IAppLogger<LimiteClienteController> _appLogger;
    private readonly ICadastrarLimiteUseCase _cadastrarLimiteUseCase;
    private readonly IAtualizarLimiteUseCase _atualizarLimiteUseCase;
    private readonly IRemoverLimiteUseCase _removerLimiteUseCase;
    private readonly IBuscarLimiteUseCase _buscarLimiteUseCase;

    public LimiteClienteController(Application.Repository.IAppLogger<LimiteClienteController> appLogger,
        ICadastrarLimiteUseCase cadastrarLimiteUseCase,
        IAtualizarLimiteUseCase atualizarLimiteUseCase,
        IRemoverLimiteUseCase removerLimiteUseCase,
        IBuscarLimiteUseCase buscarLimiteUseCase)
    {
        _appLogger = appLogger;
        _cadastrarLimiteUseCase = cadastrarLimiteUseCase;
        _atualizarLimiteUseCase = atualizarLimiteUseCase;
        _removerLimiteUseCase = removerLimiteUseCase;
        _buscarLimiteUseCase = buscarLimiteUseCase;
    }

    [HttpPost("store")]
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

    [HttpPatch("update/{documento}")]
    [ProducesResponseType(typeof(AtualizarLimiteOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(
        [FromRoute] string documento,
        [FromBody] AtualizarLimiteInput input,
        CancellationToken cancellationToken)
    {
        var output = await _atualizarLimiteUseCase.Execute(
            input,
            cancellationToken);

        return Ok(output);
    }

    [HttpDelete("delete/{documento}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(
        [FromRoute] string documento,
        CancellationToken cancellationToken)
    {
        var output = await _removerLimiteUseCase.Execute(
            new RemoverLimiteInput(documento),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{documento}")]
    [ProducesResponseType(typeof(BuscarLimiteOutput), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Buscar(
        [FromRoute] string documento,
        CancellationToken cancellationToken)
    {
        var output = await _buscarLimiteUseCase.Execute(
            new BuscarLimiteInput(documento),
            cancellationToken);

        return Ok(output);
    }
}