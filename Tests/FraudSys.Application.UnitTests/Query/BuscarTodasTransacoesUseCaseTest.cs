namespace FraudSys.Application.UnitTests.Query;

public class BuscarTodasTransacoesUseCaseTest
{
    private readonly Mock<IAppLogger<BuscarTodasTransacoesUseCase>> _appLogger;
    private readonly Mock<ITransacaoRepository> _transacaoRepositorySuccess;
    private readonly Mock<ITransacaoRepository> _transacaoRepositoryGetAllFail;
    private readonly CancellationToken _cancellationToken;

    public BuscarTodasTransacoesUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<BuscarTodasTransacoesUseCase>>();
        _transacaoRepositorySuccess = new Mock<ITransacaoRepository>();
        _transacaoRepositoryGetAllFail = new Mock<ITransacaoRepository>();

        _cancellationToken = CancellationToken.None;

        _transacaoRepositorySuccess
            .Setup(x => x.GetAllAsync(_cancellationToken))
            .ReturnsAsync(TransacaoFixture.TransacoesValidas(4));

        _transacaoRepositoryGetAllFail
            .Setup(x => x.GetAllAsync(_cancellationToken))
            .Throws(new Exception("Erro de repository"));
    }

    [Fact]
    public async Task Given_BuscarTodasTransacoesInput_When_Execute_Then_ReturnBuscarTodasTransacoesOutput()
    {
        // Arrange
        var input = new BuscarTodasTransacoesInput();

        var buscarTodasTransacoesUseCase = new BuscarTodasTransacoesUseCase(
            _appLogger.Object,
            _transacaoRepositorySuccess.Object);

        // Act
        var output = await buscarTodasTransacoesUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(4, output.data.Count());

        _transacaoRepositorySuccess
            .Verify(x => x.GetAllAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_BuscarTodasTransacoesInput_When_GetAllFails_Then_ThrowException()
    {
        // Arrange
        var input = new BuscarTodasTransacoesInput();

        var buscarTodasTransacoesUseCase = new BuscarTodasTransacoesUseCase(
            _appLogger.Object,
            _transacaoRepositoryGetAllFail.Object);

        // Act
        var act = async () => _ = await buscarTodasTransacoesUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);

        _transacaoRepositoryGetAllFail
            .Verify(x => x.GetAllAsync(_cancellationToken), Times.Once);
    }
}