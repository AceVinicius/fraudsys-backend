namespace FraudSys.Service.API.Controller;

[ApiController]
[Route("api/transacao")]
public class TransacaoController : ControllerBase
{
    private readonly IAppLogger<LimiteClienteController> _appLogger;
    private readonly IEfetuarTransacaoUseCase _efetuarTransacaoUseCase;
    private readonly IBuscarTodasTransacoesUseCase _buscarTodasTransacoesUseCase;

    public TransacaoController(
        IAppLogger<LimiteClienteController> appLogger,
        IEfetuarTransacaoUseCase efetuarTransacaoUseCase,
        IBuscarTodasTransacoesUseCase buscarTodasTransacoesUseCase)
    {
        _appLogger = appLogger;
        _efetuarTransacaoUseCase = efetuarTransacaoUseCase;
        _buscarTodasTransacoesUseCase = buscarTodasTransacoesUseCase;
    }

    [HttpPost]
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BuscarTodasTransacoesOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuscarTodosLimites(
        CancellationToken cancellationToken)
    {
        var output = await _buscarTodasTransacoesUseCase.Execute(
            new BuscarTodasTransacoesInput(),
            cancellationToken);

        return Ok(output);
    }
}